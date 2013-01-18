//
// $Id: NearestNeighbor.cs 21954 2010-11-03 08:27:35Z xpuser $
//

using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MachineLearning.NET.Data.Distances;
using MachineLearning.NET.Data.IO;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Classification
{
    /// <summary>
    /// A generic implementation of the NearestNeighbor algorithm for a generic kind of pattern.
    /// Please mind that specific versions of this class are also present for SparsePattern and SparseBag.
    /// </summary>
    /// <typeparam name="P">the type that represents the pattern</typeparam>
    /// <date>2009/12/22</date>
    /// <creator>Moreno Carullo</creator>
    public class NearestNeighbor<P> : BaseClassifier<P>
        where P:class, IGenericPattern
    {
        private static readonly ILog s_log = LogManager.GetLogger(typeof (NearestNeighbor<P>));

        protected readonly IDistance<P> m_distanceMetric;

        protected IEnumerable<P> m_patterns = new P[0];

        protected readonly int m_K = 1;

        private readonly bool m_isWeighted;

        public override bool IsTrained
        {
            get { return true; }
        }

        public NearestNeighbor(IDictionary<string, string> configuration)
        {
            if(configuration.ContainsKey("K"))
            {
                m_K = int.Parse(configuration["K"]);
            }
            if(configuration.ContainsKey("weighted"))
            {
                m_isWeighted = bool.Parse(configuration["weighted"]);
            }

            string distanceName = "euclidean";
            if(configuration.ContainsKey("distance"))
            {
                distanceName = configuration["distance"];
            }

            m_distanceMetric = DistanceFactory.GetDistanceMetric<P>(distanceName);
        }

        public override void Train(IEnumerable<P> trainingPatterns)
        {
            s_log.InfoFormat("Train started");

            var patternCount = 0;
            var patterns = new HashSet<P>();
            foreach (var pattern in trainingPatterns)
            {
                patterns.Add(pattern);
                patternCount++;
            
            }
            m_patterns = patterns;

            s_log.InfoFormat("Train finished. {0} patterns seen, {1} unique", patternCount, patterns.Count);
        }

        public override void Forget()
        {
            m_patterns = new P[0];
        }

        /// <summary>
        /// Predict the winner with a weighted-K-NN scheme.
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public override int PredictWinner(P pattern)
        {
            if(m_K == 1)
            {
                return PredictWinnerKOne(pattern);
            }

            return PredictWinnerGeneric(pattern);
        }

        /// <summary>
        /// Predict the winning class when K == 1
        /// </summary>
        /// <param name="pattern">the pattern</param>
        /// <returns>the predicted class</returns>
        private int PredictWinnerKOne(P pattern)
        {
            var winningClass = -1;
            var minDistance = double.MaxValue;
            foreach(var p in m_patterns)
            {
                var distance = m_distanceMetric.Distance(p, pattern);
                if(distance < minDistance)
                {
                    winningClass = p.Label;
                    minDistance = distance;
                }
            }
            return winningClass;
        }

        /// <summary>
        /// Predict the winning class when K > 1
        /// </summary>
        /// <param name="pattern">the pattern</param>
        /// <returns>the predicted class</returns>
        private int PredictWinnerGeneric(P pattern)
        {
            var distances = (from p in m_patterns
                             let distance = m_distanceMetric.Distance(p, pattern)
                             orderby distance
                             select new
                                        {
                                            Class = p.Label,
                                            Vote = m_isWeighted ? Math.Exp(-distance) : 1
                                        }).Take(m_K).ToArray();

            if(distances.Length == 0)
            {
                return -1;
            }

            var votes = new Dictionary<int, double>();
            foreach(var vote in distances)
            {
                if(!votes.ContainsKey(vote.Class))
                {
                    votes.Add(vote.Class, vote.Vote);
                }
                else
                {
                    votes[vote.Class] += vote.Vote;
                }
            }

            return (from v in votes orderby v.Value descending select v.Key).FirstOrDefault();
        }

        public override IDictionary<int, float> Predict(P pattern)
        {
            return new Dictionary<int, float>{{PredictWinner(pattern), 1.0f}};
        }

        public override void Save(string modelFilePath)
        {
            using(var dw = DatasetWriterFactory.GetDatasetWriter<P>(modelFilePath))
            {
                foreach(var pattern in m_patterns)
                {
                    dw.Write(pattern);
                }
            }
        }

        public override void Load(string modelFilePath)
        {
            using(var dr = DatasetReaderFactory.GetDatasetReader<P>(modelFilePath))
            {
                Train(dr.ToEnumerable());
            }
        }
    }
}