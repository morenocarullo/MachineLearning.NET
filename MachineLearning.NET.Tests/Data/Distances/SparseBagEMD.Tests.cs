using System.Collections.Generic;
using MachineLearning.NET.Data.Distances;
using MachineLearning.NET.Data.Patterns;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Data.Distances
{
    [TestFixture]
	public class SparseBagEMD_Tests
    {
        private readonly IDistance<SparseBag> m_emdDistance = DistanceFactory.GetDistanceMetric<SparseBag>("emd");

        [Test]
        public void Distance_Reflexive()
        {
            // Given
            var bag1 = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });

            // When
            var distanceMetric = m_emdDistance;
            var distance = distanceMetric.Distance(bag1, bag1);

            // Then
            Assert.That(distance, Is.EqualTo(0.0f));
        }

        [Test]
        public void Distance_Symmetric()
        {
            // Given
            var bag1 = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });

            var bag2 = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 0.0f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 0.0f}}
                                                      });

            // When
            var distanceMetric = m_emdDistance;
            var distanceA = distanceMetric.Distance(bag1, bag2);
            var distanceB = distanceMetric.Distance(bag2, bag1);

            // Then
            Assert.That(distanceA, Is.EqualTo(distanceB));
        }

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
            var distanceMetric = m_emdDistance;
            var distanceA = distanceMetric.Distance(bag1, bag2);
            var distanceB = distanceMetric.Distance(bag2, bag1);

            // Then
            Assert.That(distanceA, Is.EqualTo(distanceB));
        }
    }
}
