using System.Collections.Generic;
using MachineLearning.NET.Data.Patterns;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Data.Patterns
{
    [TestFixture]
	public class SparseBag_Tests
    {
        [Test]
        public void HasValidIstances_True_OnlyNotNullFeatures()
        {
            // Given
            var bag = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}}
                                                      });

            // When/Then
            Assert.That(bag.HasValidInstances, Is.True);
        }

        [Test]
        public void HasValidIstances_True_ButWithNullFeatures()
        {
            // Given
            var bag = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 0.0f}, {2, 0.0f}}
                                                      });

            // When/Then
            Assert.That(bag.HasValidInstances, Is.True);
        }

        [Test]
        public void HasValidIstances_False()
        {
            // Given
            var bag = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 0.0f}, {2, 0.0f}}
                                                      });

            // When/Then
            Assert.That(bag.HasValidInstances, Is.False);
        }

        [Test]
        public void RemoveEmptyIstances_WithSomethingToRemove()
        {
            // Given
            var bag = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 0.0f}, {2, 0.0f}},
                                                          new Dictionary<int, float> {{4, 9.0f}, {5, 0.0f}}
                                                      });
            var toObtain = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{4, 9.0f}}
                                                      });

            // When
            var simplifiedIstance = bag.RemoveEmptyIstances();

            // Then
            Assert.That(simplifiedIstance, Is.Not.SameAs(bag));
            Assert.That(simplifiedIstance, Is.EqualTo(toObtain));
        }

        [Test]
        public void RemoveEmptyIstances_WithNothingToRemove()
        {
            // Given
            var bag = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                      });
            var toObtain = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}}
                                                      });

            // When
            var simplifiedIstance = bag.RemoveEmptyIstances();

            // Then
            Assert.That(simplifiedIstance, Is.EqualTo(toObtain));
        }

        [Test]
        public void Equals_True()
        {
            // Given
            var bag1 = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });
            var bag2 = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });

            // When / Then
            Assert.That(bag1.Equals(bag2), Is.True);
        }

        [Test]
        public void Equals_True_DifferentBagName()
        {
            // Given
            var bag1 = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });
            var bag2 = new SparseBag(1, "2", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });

            // When / Then
            Assert.That(bag1.Equals(bag2), Is.True);
        }

        [Test]
        public void Equals_False()
        {
            // Given
            var bag1 = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });
            var bag2 = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1386f}, {2, 34.5393f}}
                                                      });

            // When / Then
            Assert.That(bag1.Equals(bag2), Is.False);
        }

        [Test]
        public void Equals_DifferentLabel_False()
        {
            // Given
            var bag1 = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });
            var bag2 = new SparseBag(-1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });

            // When / Then
            Assert.That(bag1.Equals(bag2), Is.False);
        }

        [Test]
        public void EqualOperator_True()
        {
            // Given
            var bag1 = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });
            var bag2 = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });

            // When / Then
            Assert.That(bag1 == bag2);
        }

        [Test]
        public void EqualOperator_False()
        {
            // Given
            var bag1 = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });
            var bag2 = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1386f}, {2, 34.5393f}}
                                                      });

            // When / Then
            Assert.That(bag1 != bag2);
        }
    }
}
