using System.Collections.Generic;
using System.Linq;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Classification.MultiInstance
{
    [Classifier(Name = "ocnn")]
    internal class OptimizedCitationNearestNeighbor : CitationNearestNeighbor
    {
        public OptimizedCitationNearestNeighbor(IDictionary<string, string> configuration) : base(configuration)
        {
        }

        protected override HashSet<SparseBag> GetNearest(IEnumerable<SparseBag> bags, SparseBag bagToTest, int K)
        {
            var topBags = new BagAndDistance[K];
            var maxDistance = double.MaxValue;
            var maxDistancePos = 0;

            foreach (var bag in bags)
            {
                if (bag == bagToTest) continue;
                var distance = m_distanceMetric.Distance(bag, bagToTest);

                // The farthest value has to be replaced. Recompute next
                if (distance < maxDistance)
                {
                    topBags[maxDistancePos] = new BagAndDistance { bag = bag, Distance = distance };

                    maxDistance = double.MinValue;
                    for (var bagId = 0; bagId < topBags.Length; bagId++)
                    {
                        if (topBags[bagId].bag != null && topBags[bagId].Distance > maxDistance)
                        {
                            maxDistance = topBags[bagId].Distance;
                            maxDistancePos = bagId;
                        }
                    }
                }
            }

            return new HashSet<SparseBag>((from b in topBags where b.bag != null orderby b.Distance select b.bag).Take(K));
        }

        struct BagAndDistance
        {
            public double Distance;
            public SparseBag bag;
        }
    }
}
