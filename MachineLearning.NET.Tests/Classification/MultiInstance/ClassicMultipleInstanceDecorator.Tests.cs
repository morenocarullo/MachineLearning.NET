using System.Collections.Generic;
using MachineLearning.NET.Classification;
using MachineLearning.NET.Data.Patterns;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Classification.MultiInstance
{
    [TestFixture]
	public class ClassicMultipleInstanceDecorator_Tests
    {
        [Test]
        public void PredictWinner_SparseBagsAreSingletons_PositiveWins()
        {
            // Given
            var bags = new[]
                           {
                               new BagTestData(1, 2, new[]
                                                         {
                                                             new[] {26.4386, 25.5379}
                                                         }),
                               new BagTestData(1, 3, new[]
                                                         {
                                                             new[] {44.9671, 32.1093}
                                                         }),
                               new BagTestData(-1, 4, new[]
                                                          {
                                                              new[] {20.7886, 17.2379}
                                                          })
                           };
            var bagToTest = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });

            var miClassifier = ClassifierFactory.GetMultiInstanceClassifier("mi:model=svm,gamma=123");

            // When
            var predictedLabel = PredictBagLabel(miClassifier, bags, bagToTest);

            // Then
            Assert.That(predictedLabel, Is.EqualTo(1));
        }

        [Test]
        public void PredictWinner_SparseBagsAreSingletons_NegativeWins()
        {
            // Given
            var bags = new[]
                           {
                               new BagTestData(-1, 2, new[]
                                                         {
                                                             new[] {26.4386, 25.5379}
                                                         }),
                               new BagTestData(-1, 3, new[]
                                                         {
                                                             new[] {44.9671, 32.1093}
                                                         }),
                               new BagTestData(+1, 4, new[]
                                                          {
                                                              new[] {20.7886, 17.2379}
                                                          })

                           };
            var bagToTest = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });

            var miClassifier = ClassifierFactory.GetMultiInstanceClassifier("mi:model=svm,gamma=123");

            // When
            var predictedLabel = PredictBagLabel(miClassifier, bags, bagToTest);

            // Then
            Assert.That(predictedLabel, Is.EqualTo(-1));
        }

        #region Utility stuff

        private static int PredictBagLabel(IClassifier<SparseBag> classifier, BagTestData[] bags, SparseBag bagToTest)
        {
            var sparseBags = BagTestData.ToSparseBags(bags);

            classifier.Train(sparseBags);

            return classifier.PredictWinner(bagToTest);
        }

        #endregion
    }
}
