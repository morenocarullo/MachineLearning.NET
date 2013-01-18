using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Globalization;
using System.Runtime.CompilerServices;
using log4net;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Classification.Classic
{
    /// <summary>
    /// A Naive Bayes classifier. This supposes that SparsePattern(s) are built with a proper technique,
    /// where each feature is the term count.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2009/02/24</date>
    [Classifier(Name="naivebayes")]
    public class NaiveBayesClassifier : BaseClassifier<SparsePattern>
    {
        private static readonly ILog s_log = LogManager.GetLogger(typeof(NaiveBayesClassifier));
	
        /// <summary>
        /// Preserve culture-invariant formatting of numbers when serializing.
        /// </summary>
        private static readonly NumberFormatInfo s_numberFormat = CultureInfo.InvariantCulture.NumberFormat;
	
        /// <summary>
        /// The class priors P(c)
        /// </summary>
        private readonly IDictionary<int, double> m_classPriors = new Dictionary<int, double>();

        private readonly IDictionary<int, int> m_classTokenCount = new Dictionary<int, int>();
        private readonly HashSet<int> m_knownTerms = new HashSet<int>();

        /// <summary>
        /// The conditional probabilities P(t|c)
        /// </summary>
        private readonly IDictionary<int, float[]> m_tokenCondProbability = new Dictionary<int, float[]>();

        /// <summary>
        /// The greatest term ID. It is used to optimize in-memory structures.
        /// </summary>
        private int m_maxTermId;

        public override bool IsTrained
        {
            get
            {
                return m_tokenCondProbability.Count > 0;
            }
        }

        public NaiveBayesClassifier(IDictionary<string, string> configuration)
        {
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Train(IEnumerable<SparsePattern> patterns)
        {
            var totalDocs = 0;
            m_maxTermId = 0;

            s_log.Info("First-pass statistics...");
            foreach (var pattern in patterns)
            {
                // Estimate P(c)
                if (!m_classPriors.ContainsKey(pattern.Label)) m_classPriors.Add(pattern.Label, 1);
                else m_classPriors[pattern.Label] += 1;

                // Collect terms
                foreach (var term in pattern.Features)
                {
                    if (term.Key > m_maxTermId)
                    {
                        m_maxTermId = term.Key;
                    }
                    m_knownTerms.Add(term.Key);
                }

                totalDocs++;
            }

            s_log.InfoFormat("{0} docs, {1} terms, {2} classes.", totalDocs, m_maxTermId, m_classPriors.Count);

            var requiredMbs = (((long)m_maxTermId * m_classPriors.Count) / (1024 * 1024)) * sizeof(float);
            s_log.InfoFormat("Minimum required memory: {0} Mb", requiredMbs);

            s_log.Info("Second-pass statistics...");

            foreach (var pattern in patterns)
            {
                // Setup structures for P(t|c)
                if (!m_tokenCondProbability.ContainsKey(pattern.Label))
                {
                    // Add statistics structure for each term
                    m_tokenCondProbability.Add(pattern.Label, new float[m_maxTermId + 1]);
                }

                // Estimate P(t|c)
                foreach (var term in pattern.Features)
                {
                    var termId = term.Key;
                    var termFrequency = (int) term.Value;

                    // Count T_ct
                    m_tokenCondProbability[pattern.Label][termId] += termFrequency;

                    // Count total tokens in class C for normalization
                    if (!m_classTokenCount.ContainsKey(pattern.Label))
                    {
                        m_classTokenCount.Add(pattern.Label, termFrequency);
                    }
                    else
                    {
                        m_classTokenCount[pattern.Label] += termFrequency;
                    }
                }
            }


            // Final computation for P(c)
            foreach (var classId in new List<int>(m_classPriors.Keys))
            {
                m_classPriors[classId] = Math.Log(m_classPriors[classId] / totalDocs);
            }

            s_log.InfoFormat("Class priors computed for {0} classes.", m_classPriors.Count);            
            s_log.InfoFormat("Finalizing class-conditionals...");

            // Final computation for P(t_k|c)
            foreach (var condProb in m_tokenCondProbability)
            {
                // For each token in this class
                foreach (var tokenId in m_knownTerms)
                {
                    // Laplace smoothing
                    condProb.Value[tokenId] =
                        (float)Math.Log((condProb.Value[tokenId] + 1) /
                                        (m_classTokenCount[condProb.Key] + m_knownTerms.Count));

                }
            }

            s_log.InfoFormat("Class-conditionals computed {0} classes, {1} terms.", m_classPriors.Count, m_knownTerms.Count);
        }

        public override void Forget()
        {
            // Clean all
            m_classPriors.Clear();
            m_tokenCondProbability.Clear();
        }

        private double GetTermCondProbability(int classId, int termId)
        {
            const double defaultValue = double.MinValue;

            if(!m_tokenCondProbability.ContainsKey(classId))
            {
                return defaultValue;
            }

            return (termId < m_tokenCondProbability[classId].Length)
                       ? m_tokenCondProbability[classId][termId] : defaultValue;
        }

        /// <summary>
        /// Predicts the belonging of a document with P(c|x) = log(P(c)) + sum_k log(P(t_k|c))
        /// </summary>
        /// <param name="sparsePattern"></param>
        /// <returns></returns>
        public override IDictionary<int, float> Predict(SparsePattern sparsePattern)
        {
            IDictionary<int, float> posteriors = new Dictionary<int, float>();
            float minPosterior = float.MaxValue, maxPosterior = float.MinValue;

            // Compute posteriors for each class
            foreach (var classes in m_classPriors)
            {
                // Init the posterior with log(P(c))
                var classPosterior = (float)classes.Value;
                var classId = classes.Key;
                foreach (var term in sparsePattern.Features.Keys)
                {
                    // add log(P(t_k|c))
                    classPosterior += (float)GetTermCondProbability(classId,term);
                }

                // 1. find the minimum value.
                if (classPosterior > maxPosterior)
                {
                    maxPosterior = classPosterior;
                }

                // 2. find the maximum value.
                if (classPosterior < minPosterior)
                {
                    minPosterior = classPosterior;
                }

                posteriors.Add(classId, classPosterior);
            }

            // sum-to-one
            if (maxPosterior != minPosterior)
            {
                foreach (var classId in m_classPriors.Keys)
                {
                    posteriors[classId] = (posteriors[classId] - minPosterior)/(maxPosterior - minPosterior);
                }
            }
            else
            {
                foreach (var classId in m_classPriors.Keys)
                {
                    posteriors[classId] = posteriors[classId]/m_classPriors.Keys.Count;
                }
            }

            return posteriors;
        }

        public override void Save(string sModelFilePath)
        {
            // save m_classPriors
            // p 10 0.11
            //    ^class ^condProb
            // save conditioned_probabilities
            // c 10 1011 0.12
            //   ^class ^token ^condProb

            using (var fileStream = new FileStream(sModelFilePath, FileMode.Create))
            using (var gzipStream = new GZipStream(fileStream, CompressionMode.Compress))
            using (var sw = new StreamWriter(gzipStream))
            {
                sw.WriteLine("t {0}", m_maxTermId);

                foreach(var prior in m_classPriors)
                {
                    sw.WriteLine("p {0} {1}", prior.Key, prior.Value);
                }
				
                foreach(var condProb in m_tokenCondProbability)
                {
                    for (var termId = 0; termId < m_maxTermId+1; termId++)
                    {
                        sw.WriteLine("{0} {1} {2}", condProb.Key, termId, condProb.Value[termId]);
                    }
                }

                sw.Flush();
            }
        }

        public override void Load(string sModelFilePath)
        {
            Forget();

            using (var fileStream = new FileStream(sModelFilePath, FileMode.Open))
            using (var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
            using (var sr = new StreamReader(gzipStream))
            {
                string line;
                while ( (line=sr.ReadLine()) != null )
                {
                    /**/ if( line[0] == 't' )
                    {
                        var parts = line.Split(' ');
                        m_maxTermId = int.Parse(parts[1], s_numberFormat);
                    }
                    else if( line[0] == 'p' )
                    {
                        var parts = line.Split(' ');
                        var classId = int.Parse(parts[1], s_numberFormat);
                        var prior = double.Parse(parts[2], s_numberFormat);
                        m_classPriors.Add(classId, prior);
                    }
                    else
                    {
                        var parts = line.Split(' ');
                        var classId = int.Parse(parts[0], s_numberFormat);
                        var tokenId = int.Parse(parts[1], s_numberFormat);
                        var condProb = double.Parse(parts[2], s_numberFormat);
                        if( !m_tokenCondProbability.ContainsKey(classId) )
                        {
                            m_tokenCondProbability.Add(classId, new float[m_maxTermId+1]);
                        }

                        m_tokenCondProbability[classId][tokenId] = (float)condProb;
                    }
                }
            }
        }
    }
}