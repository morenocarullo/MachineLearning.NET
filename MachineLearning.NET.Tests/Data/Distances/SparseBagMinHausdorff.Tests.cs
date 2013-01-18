using System.Collections.Generic;
using MachineLearning.NET.Data.Distances;
using MachineLearning.NET.Data.Patterns;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Data.Distances
{
    [TestFixture]
	public class SparseBagMinHausdorff_Tests
    {
        private readonly IDistance<SparseBag> m_minHausdorffDistance = DistanceFactory.GetDistanceMetric<SparseBag>("minhausdorff");

        [Test]
        public void Distance_Zero()
        {
            // Given
            var bag1 = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 1.0f}, {2, 2.0f}},
                                                          new Dictionary<int, float> {{1, 100.0f}, {2, 200.0f}}
                                                      });

            var bag2 = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 200.0f}, {2, 100.0f}},
                                                          new Dictionary<int, float> {{1, 1.0f}, {2, 2.0f}},
                                                      });

            // When
            var distanceA = m_minHausdorffDistance.Distance(bag1, bag2);
            var distanceB = m_minHausdorffDistance.Distance(bag2, bag1);

            // Then
            Assert.That(distanceA, Is.EqualTo(distanceB));
        }

        [Test]
        public void Distance_RealCase_1()
        {
            var istances1 = new[]
                                {
                                    new Dictionary<int, float>
                                        {
                                            {1, 1}
                                        },
                                    new Dictionary<int, float>
                                        {
                                            {1, 2}
                                        },
                                    new Dictionary<int, float>
                                        {
                                            {1, 3}
                                        }
                                };

            var bag1 = new SparseBag(0, "a", istances1);

            var istances2 = new[]
                                {
                                    new Dictionary<int, float>
                                        {
                                            {1, 4}
                                        },
                                    new Dictionary<int, float>
                                        {
                                            {1, 5}
                                        },
                                    new Dictionary<int, float>
                                        {
                                            {1, 6}
                                        }
                                };

            var bag2 = new SparseBag(0, "a", istances2);

            // When
            var distance = m_minHausdorffDistance.Distance(bag1, bag2);

            // Then
            Assert.That(distance, Is.EqualTo(1));
        }

        [Test]
        public void Distance_RealCase_2()
        {
            var istances1 = new[]
                                {
                                    new Dictionary<int, float>
                                        {
                                            {1, 15}
                                        },
                                    new Dictionary<int, float>
                                        {
                                            {1, 2}
                                        },
                                };

            var bag1 = new SparseBag(0, "a", istances1);

            var istances2 = new[]
                                {
                                    new Dictionary<int, float>
                                        {
                                            {1, 4}
                                        },
                                    new Dictionary<int, float>
                                        {
                                            {1, 20}
                                        }
                                };

            var bag2 = new SparseBag(0, "a", istances2);

            // When
            var distance = m_minHausdorffDistance.Distance(bag1, bag2);

            // Then
            Assert.That(distance, Is.EqualTo(2));
        }
    }
}
