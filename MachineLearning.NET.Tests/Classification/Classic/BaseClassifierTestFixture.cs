using System.Collections.Generic;
using System.IO;
using System.Linq;
using MachineLearning.NET.Classification;
using MachineLearning.NET.Data.IO;
using MachineLearning.NET.Data.Patterns;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Classification.Classic
{
	public class BaseClassifierTestFixture
    {
        protected static void Train_SimpleCase(string classifierConfig)
        {
            // Given
            var patterns = new[]
                               {
                                   new SparsePattern(new Dictionary<int, float> {{1,0.0f}, {2,0.8f} }, 1),
                                   new SparsePattern(new Dictionary<int, float> {{1,0.1f}, {2,0.85f}}, 1),
                                   new SparsePattern(new Dictionary<int, float> {{1,0.9f}, {2,0.85f}}, 1),
                                   new SparsePattern(new Dictionary<int, float> {{2,0.0f}, {1,0.8f} }, 2),
                                   new SparsePattern(new Dictionary<int, float> {{2,0.1f}, {1,0.85f}}, 2),
                                   new SparsePattern(new Dictionary<int, float> {{2,0.1f}, {1,0.84f}}, 2)
                               };
            var classifier = ClassifierFactory.GetClassicClassifier(classifierConfig);

            // When
            classifier.Train(patterns);

            // Then
            Assert.That(classifier.PredictWinner(patterns[0]), Is.EqualTo(1));
            Assert.That(classifier.PredictWinner(patterns[3]), Is.EqualTo(2));
        }

        protected static void Train_Test_Iris(string classifierConfig)
        {
            // Given
            var datasetFilePath = Path.Combine(TestConstants.TestFilesPath, "iris.sparse");
            var classifier = ClassifierFactory.GetClassicClassifier(classifierConfig);
            var irisPatterns = DatasetReaderFactory.GetDatasetReader<SparsePattern>(datasetFilePath).ToList();

            // When
            classifier.Train(irisPatterns);
            var qualityEvaluator = ClassifierUtils<SparsePattern>.Test(classifier, datasetFilePath);

            // Then
            Assert.That(qualityEvaluator.GetMetrics().ContainsKey("OA"));
            Assert.That(qualityEvaluator.GetMetrics()["OA"], Is.GreaterThan(0.80f));
        }
    }
}
