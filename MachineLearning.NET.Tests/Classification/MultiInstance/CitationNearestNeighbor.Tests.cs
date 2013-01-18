using System;
using System.Collections.Generic;
using MachineLearning.NET.Classification;
using MachineLearning.NET.Classification.MultiInstance;
using MachineLearning.NET.Data.IO;
using MachineLearning.NET.Data.Patterns;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Classification.MultiInstance
{

    [TestFixture]
	public class CitationNearestNeighbor_Tests
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

        [Test]
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

            int predictedLabelForOne = PredictBagLabel(bags, bagToTest, 2);


            // Then
            Assert.That(predictedLabelForOne, Is.EqualTo(-1));
        }

        [Test]
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

            int predictedLabelForOne = PredictBagLabel(bags, bagToTest, 2);


            // Then
            Assert.That(predictedLabelForOne, Is.EqualTo(-1));
        }

        [Test]
        public void PredictWinner_NegativeWins_EmptyKnowledge()
        {
            // Given
            var bags = new BagTestData[0];
            var bagToTest = new SparseBag(1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });

            int predictedLabelForOne = PredictBagLabel(bags, bagToTest, 2);


            // Then
            Assert.That(predictedLabelForOne, Is.EqualTo(-1));
        }

        [Test]
        public void PredictWinner_PositiveWins_AllPositiveSamples()
        {
            // Given
            var bags = new[]
                           {
                               new BagTestData(+1, 1, new[]
                                                          {
                                                              new[] {22.8529, 22.6664},
                                                              new[]{59.1986, 34.5393}
                                                          }),
                               new BagTestData(+1, 2, new[]
                                                          {
                                                              new[] {26.4386, 25.5379}
                                                          }),
                               new BagTestData(+1, 3, new[]
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

            int predictedLabelForOne = PredictBagLabel(bags, bagToTest, 2);


            // Then
            Assert.That(predictedLabelForOne, Is.EqualTo(+1));
        }

        [Test]
        public void PredictWinner_NegativeWins_AllNegativeSamples()
        {
            // Given
            var bags = new[]
                           {
                               new BagTestData(-1, 1, new[]
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
            Assert.That(predictedLabelForOne, Is.EqualTo(-1));
        }

        [Test]
        [ExpectedException(typeof(NotBinaryProblemException))]
        public void PredictWinner_NotBinary()
        {
            // Given
            var bags = new[]
                           {
                               new BagTestData(-1, 2, new[]
                                                          {
                                                              new[] {26.4386, 25.5379}
                                                          }),
                               new BagTestData(1, 3, new[]
                                                          {
                                                              new[] {44.9671, 32.1093}
                                                          }),
                               new BagTestData(2, 4, new[]
                                                         {
                                                             new[] {20.7886, 17.2379}
                                                         })

                           };
            var bagToTest = new SparseBag(-1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });

            PredictBagLabel(bags, bagToTest, 3);
        }

        [Test, Ignore("This is a performance test")]
        public void Test_Musk1Speed()
        {
            // Given
            const int K = 2;
            var citationNearestNeighbor = ClassifierFactory.GetMultiInstanceClassifier("ocnn:distance=hausdorff,K=" + K);
            const string datasetPath = "../../TestFiles/sparsebags/musk1.data";
            var bagToTest = new SparseBag(-1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });

            citationNearestNeighbor.Train(DatasetReaderFactory.GetDatasetReader<SparseBag>(datasetPath));

            var start = DateTime.Now;
            citationNearestNeighbor.PredictWinner(bagToTest);
            
            Console.WriteLine("Total time: {0}", (DateTime.Now-start));
        }

        #region Utility stuff

        private static int PredictBagLabel(BagTestData[] bags, SparseBag bagToTest, int K)
        {
            var sparseBags = BagTestData.ToSparseBags(bags);

            var citationNearestNeighbor = ClassifierFactory.GetMultiInstanceClassifier("cnn:distance=hausdorff,K="+K);

            citationNearestNeighbor.Train(sparseBags);

            return citationNearestNeighbor.PredictWinner(bagToTest);
        }

        #endregion
    }
}
