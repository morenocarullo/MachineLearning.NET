using System.Collections.Generic;
using MachineLearning.NET.Classification;
using MachineLearning.NET.Data.Patterns;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Classification.MultiInstance
{
    [TestFixture]
	public class RBFNClassifier_Tests
    {
        [Test]
        public void PredictWinner_PositiveWins()
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

            int predictedLabelForOne = PredictBagLabel(bags, bagToTest, 2);


            // Then
            Assert.That(predictedLabelForOne, Is.EqualTo(+1));
        }

        [Test, Ignore("Investigate SIGMA parameter")]
        public void PredictWinner_PositiveWins_TestedSampleIsInTrS()
        {
            // Given
            var bags = new[]
                           {
                               new BagTestData(-1, 1, new[]
                                                          {
                                                              new[] {22.8529, 22.6664f},
                                                              new[] {59.1986, 34.5393f}
                                                          }),
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
            var bagToTest = new SparseBag(-1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });

            int predictedLabelForOne = PredictBagLabel(bags, bagToTest, 2);


            // Then
            Assert.That(predictedLabelForOne, Is.EqualTo(+1));
        }

        [Test]
        public void PredictWinner_NegativeWins()
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
                               new BagTestData(1, 4, new[]
                                                         {
                                                             new[] {20.7886, 17.2379}
                                                         })

                           };
            var bagToTest = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });

            var predictedLabelForOne = PredictBagLabel(bags, bagToTest, 2);


            // Then
            Assert.That(predictedLabelForOne, Is.EqualTo(-1));
        }

        [Test, Ignore("Investigate SIGMA parameter")]
        public void PredictWinner_NegativeWins_TestedSampleInTrS()
        {
            // Given
            var bags = new[]
                           {
                               new BagTestData(+1, 1, new[]
                                                          {
                                                              new[] {22.8529, 22.6664},
                                                              new[]{59.1986, 34.5393}
                                                          }),
                               new BagTestData(-1, 2, new[]
                                                          {
                                                              new[] {26.4386, 25.5379}
                                                          }),
                               new BagTestData(-1, 3, new[]
                                                          {
                                                              new[] {44.9671, 32.1093}
                                                          }),
                               new BagTestData(1, 4, new[]
                                                         {
                                                             new[] {20.7886, 17.2379}
                                                         })

                           };
            var bagToTest = new SparseBag(+1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });

            var predictedLabelForOne = PredictBagLabel(bags, bagToTest, 3);


            // Then
            Assert.That(predictedLabelForOne, Is.EqualTo(-1));
        }

        private static int PredictBagLabel(BagTestData[] bags, SparseBag bagToTest, int M)
        {
            var sparseBags = BagTestData.ToSparseBags(bags);

            var citationNearestNeighbor = ClassifierFactory.GetMultiInstanceClassifier("rbfn:distance=hausdorff,M=" + M+",sigma=1");

            citationNearestNeighbor.Train(sparseBags);

            return citationNearestNeighbor.PredictWinner(bagToTest);
        }
    }
}
