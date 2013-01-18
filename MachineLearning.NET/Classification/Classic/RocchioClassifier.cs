using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using MachineLearning.NET.Data.IO;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Classification.Classic
{
    /// <summary>
    /// A basic classifier that builds a mean vector for each class and uses
    /// the dot-product as similarity measure.
    /// This method, though simple, can scale very well with the number of
    /// features.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2008/07/14</date>
    [Classifier(Name = "rocchio")]
    public class RocchioClassifier : BaseClassifier<SparsePattern>
    {
        private IDictionary<int, ClassCentroid> m_nMeans = new Dictionary<int, ClassCentroid>();

        public override bool IsTrained
        {
            get { return m_nMeans.Count > 0; }
        }

        public RocchioClassifier(IDictionary<string, string> configuration)
        {

        }

        public override void Forget()
        {
            m_nMeans = new Dictionary<int, ClassCentroid>();
        }

        public override void Train(IEnumerable<SparsePattern> patterns)
        {
            foreach (var pattern in patterns)
            {
                if (!m_nMeans.ContainsKey(pattern.Label))
                {
                    m_nMeans.Add(pattern.Label, new ClassCentroid());
                }
                else
                {
                    m_nMeans[pattern.Label].Add(pattern.Features);
                }
            }

            foreach (var centroid in m_nMeans.Values)
            {
                centroid.ComputeMean();
            }
        }

        public override IDictionary<int, float> Predict(SparsePattern sparsePattern)
        {
            IDictionary<int, float> predictions = new Dictionary<int, float>();
            IDictionary<int, float> normPredictions = new Dictionary<int, float>();
            var fTotalLength = 0f;

            foreach (var nClass in m_nMeans.Keys)
            {
                var fSimilarity = m_nMeans[nClass].Similarity(sparsePattern.Features);
                predictions.Add(nClass, fSimilarity);
                fTotalLength += fSimilarity;
            }

            foreach (var kv in predictions)
            {
                normPredictions.Add(kv.Key, kv.Value / fTotalLength);
            }

            return normPredictions;
        }

        /// <summary>
        /// Save the trained model by serializing the centroid object.
        /// </summary>
        /// <param name="sModelFilePath">
        /// A <see cref="System.String"/> with the path to the serialized object.
        /// </param>
        public override void Save(string sModelFilePath)
        {
            var fileStream = new FileStream(sModelFilePath, FileMode.Create);
            var gzipStream = new GZipStream(fileStream, CompressionMode.Compress);
            var sw = new StreamWriter(gzipStream);

            foreach (var kv in m_nMeans)
            {
                sw.Write("{0} ", kv.Key);
                foreach (var featureKv in kv.Value.Features)
                {
                    sw.Write("{0}:{1} ", featureKv.Key, featureKv.Value);
                }
                sw.WriteLine();
            }

            sw.Close();
        }

        public override void Load(string sModelFilePath)
        {
            var fileStream = new FileStream(sModelFilePath, FileMode.Open);
            var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress);

            using (var sparseDatasetReader = new SparseDatasetReader(gzipStream))
                foreach (var sp in sparseDatasetReader)
                {
                    var classCentroid = new ClassCentroid();
                    classCentroid.Add(sp.Features);
                    m_nMeans.Add(sp.Label, classCentroid);
                }
        }
    }

    /// <summary>
    /// This internal class is used by the Rocchio classifier to keep track
    /// of a class' centroid.
    /// </summary>
    [Serializable]
    class ClassCentroid
    {
        /// <summary>
        /// The mean pattern for this class.
        /// </summary>
        private IDictionary<int, float> m_nFeatures = new Dictionary<int, float>();

        /// <summary>
        /// The number of patterns that belong to this class.
        /// </summary>
        private int m_nSamples = 0;

        /// <summary>
        /// The mean pattern for this class.
        /// </summary>
        public IDictionary<int, float> Features
        {
            get
            {
                return m_nFeatures;
            }
        }

        /// <summary>
        /// Computes the similarity between this centroid and the given
        /// pattern's features.
        /// Up to now it has a built-in strategy with the dot-product as similarity
        /// meaure.
        /// </summary>
        /// <param name="features">
        /// A <see cref="IDictionary`2"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Single"/>
        /// </returns>
        public float Similarity(IDictionary<int, float> features)
        {
            var fOverallSimilarity = 0.0f;

            foreach (var nFeature in features.Keys)
            {
                if (m_nFeatures.ContainsKey(nFeature))
                {
                    fOverallSimilarity += m_nFeatures[nFeature] * features[nFeature];
                }
            }

            return fOverallSimilarity;
        }

        /// <summary>
        /// Add new pattern's features to this class centroid.
        /// </summary>
        /// <param name="features">
        /// A <see cref="IDictionary`2"/> new set of features from a new pattern.
        /// </param>
        public void Add(IDictionary<int, float> features)
        {
            foreach (var nFeature in features.Keys)
            {
                if (!m_nFeatures.ContainsKey(nFeature))
                {
                    m_nFeatures.Add(nFeature, features[nFeature]);
                }
                else
                {
                    m_nFeatures[nFeature] += features[nFeature];
                }
            }

            m_nSamples++;
        }

        /// <summary>
        /// When this is called, the actual mean is recomputed considering
        /// the number of samples of the class.
        /// </summary>
        public void ComputeMean()
        {
            IDictionary<int, float> finalFeatures = new Dictionary<int, float>();

            foreach (var nFeature in m_nFeatures.Keys)
            {
                finalFeatures.Add(nFeature, m_nFeatures[nFeature] / m_nSamples);
            }

            m_nFeatures = finalFeatures;
        }
    }
}