using System;
using System.Collections.Generic;
using MachineLearning.NET.Classification;
using MachineLearning.NET.Data.IO;
using MachineLearning.NET.Data.Patterns;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Classification.MultiInstance
{
    [TestFixture]
	public class MultiInstanceNearestNeighbor_Tests
	{
        [Test]
        public void Train_Test_NegativeWins()
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
                                                          new Dictionary<int, float> {{1, 44.9671f}, {2, 32.1093f}}
                                                      });

            var predictedLabelForOne = PredictBagLabel(bags, bagToTest, 1);


            // Then
            Assert.That(predictedLabelForOne, Is.EqualTo(-1));
        }

        [Test]
        public void Train_Test_PositiveWins()
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
                                                          new Dictionary<int, float> {{1, 20.7886f}, {2, 17.2379f}}
                                                      });

            var predictedLabelForOne = PredictBagLabel(bags, bagToTest, 1);


            // Then
            Assert.That(predictedLabelForOne, Is.EqualTo(+1));
        }

        [Test, Ignore("This is a performance test")]
        public void Test_Musk1Speed()
        {
            // Given
            const int K = 1;
            var citationNearestNeighbor = ClassifierFactory.GetMultiInstanceClassifier("nn:distance=hausdorff,K=" + K);
            const string datasetPath = "../../TestFiles/sparsebags/musk1.data";
            var bagToTest = new SparseBag(-1, "1", new[]
                                                      {
                                                          new Dictionary<int, float> {{1, 22.8529f}, {2, 22.6664f}},
                                                          new Dictionary<int, float> {{1, 59.1986f}, {2, 34.5393f}}
                                                      });

            citationNearestNeighbor.Train(DatasetReaderFactory.GetDatasetReader<SparseBag>(datasetPath));

            var start = DateTime.Now;
            citationNearestNeighbor.PredictWinner(bagToTest);

            Console.WriteLine("Total time: {0}", (DateTime.Now - start));
        }

        private static int PredictBagLabel(BagTestData[] bags, SparseBag bagToTest, int K)
        {
            var sparseBags = BagTestData.ToSparseBags(bags);

            // When
            var citationNearestNeighbor = ClassifierFactory.GetMultiInstanceClassifier("nn:distance=hausdorff,K=" + K);

            citationNearestNeighbor.Train(sparseBags);

            return citationNearestNeighbor.PredictWinner(bagToTest);
        }
	}
}
