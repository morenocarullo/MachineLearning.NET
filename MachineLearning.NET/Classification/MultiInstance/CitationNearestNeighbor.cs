//
// $Id: CitationNearestNeighbor.cs 21954 2010-11-03 08:27:35Z xpuser $
//

using System.Collections.Generic;
using System.Linq;
using log4net;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Classification.MultiInstance
{
    /// <summary>
    /// This is an implementation of Citation-kNN as described in
    /// Wang and Zucker, "Solving the Multiple-Instance Problem:A Lazy Learning Approach", ICML 2000
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2009/12/22</date>
    [Classifier(Name = "cnn")]
    public class CitationNearestNeighbor : NearestNeighbor<SparseBag>
    {
        private static readonly ILog s_log = LogManager.GetLogger(typeof(CitationNearestNeighbor));

        /// <summary>
        /// Number of citers
        /// </summary>
        private readonly int m_C;

        /// <summary>
        /// Number of references
        /// </summary>
        private readonly int m_R;

        public CitationNearestNeighbor(IDictionary<string, string> configuration)
            : base(configuration)
        {
            m_R = m_K;
            m_C = m_R + 2; // See Wang & Zucher 2000 p.5

            if (configuration.ContainsKey("C"))
            {
                m_C = int.Parse(configuration["C"]);
            }

            s_log.InfoFormat("Using C = {0} and R = {1}", m_C, m_R);
        }

        public override int PredictWinner(SparseBag spPattern)
        {
            // The R-NearestNeighbors
            var nearestReferences = GetNearest(m_patterns, spPattern, m_R);

            // Compute the C-NearestNeighbors
            var nearestCiters = new HashSet<SparseBag>();
            var allPatternsPlusUnseen = m_patterns.Union(new[] { spPattern });

            foreach (var pattern in m_patterns)
            {
                var thisPattern = pattern;
                var thisNearest = GetNearest(allPatternsPlusUnseen, thisPattern, m_C);

                // Keep only the citers. Citers contain the requested spPattern
                if (!thisNearest.Contains(spPattern))
                {
                    continue;
                }

                nearestCiters.Add(thisPattern);
            };

            // Aggregate R-NearestNeighbors and C-NearestNeighbors to predict
            var allResults = nearestReferences.Union(nearestCiters);
            var votes = (from v in allResults
                         group v by v.Label
                             into p
                             let count = p.Count()
                             orderby count descending, p.Key ascending
                             select new { Label = p.Key, NumVotes = count }).ToArray();

            if (votes.Length > 2)
            {
                throw new NotBinaryProblemException();
            }

            return votes.Any() ? votes[0].Label : -1;
        }

        protected virtual HashSet<SparseBag> GetNearest(IEnumerable<SparseBag> bags, SparseBag bagToTest, int K)
        {
            return new HashSet<SparseBag>((from p in bags
                                           let distance = m_distanceMetric.Distance(p, bagToTest)
                                           orderby distance
                                           where p != bagToTest
                                           select p).Take(K));
        }
    }
}