using System.Collections.Generic;
using MachineLearning.NET.Classification;
using MachineLearning.NET.Data.Patterns;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Classification.MultiInstance
{
    [TestFixture]
	public class RuleBasedMultiInstanceClassifier_Tests
    {
        [Test]
        public void PredictWinner_EmptyRuleSet_PositiveClassWins()
        {
            // Given
            var bagsToTrain = new SparseBag[0];
            var bagsToTest = new SparseBag(+1, "2", new[]
                                                        {
                                                            new Dictionary<int, float> {{1, 1.0f}, {2, 3.0f}}
                                                        });

            var classifier = ClassifierFactory.GetMultiInstanceClassifier("rules:class+1=");

            // When
            classifier.Train(bagsToTrain);
            var predictedClass = classifier.PredictWinner(bagsToTest);
            
            // Then
            Assert.That(predictedClass, Is.EqualTo(bagsToTest.Label));
        }

        [Test]
        public void PredictWinner_SimpleRuleSet()
        {
            // Given
            var bagsToTrain = new SparseBag[0];
            var bagsToTest = new[]
                                 {
                                     new SparseBag(-1, "1", new[]
                                                                {
                                                                    new Dictionary<int, float> {{1, 1.0f}, {2, 1.9f}}
                                                                }),
                                     new SparseBag(+1, "2", new[]
                                                                {
                                                                    new Dictionary<int, float> {{1, 1.0f}, {2, 3.0f}}
                                                                })
                                 };

            var classifier = ClassifierFactory.GetMultiInstanceClassifier("rules:class+1=1>0.9^2>2.0");

            // When
            classifier.Train(bagsToTrain);
            foreach (var bag in bagsToTest)
            {
                var predictedClass = classifier.PredictWinner(bag);

                // Then
                Assert.That(predictedClass, Is.EqualTo(bag.Label));
            }
        }
    }
}
