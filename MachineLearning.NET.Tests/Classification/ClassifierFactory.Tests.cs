using MachineLearning.NET.Classification;
using MachineLearning.NET.Classification.Classic;
using MachineLearning.NET.Classification.MultiInstance;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Classification
{
    [TestFixture]
    public class ClassifierFactory_Tests
    {
        [Test]
        public void GetClassicClassifier_ValidModels()
        {
            // Given
            var models = new[]
                             {
                                 new{ ModelString="svm:type=rbf", ExpectedType = typeof(SVMClassifier)},
                                 new{ ModelString="rocchio", ExpectedType = typeof(RocchioClassifier)},
                                 new{ ModelString="nn", ExpectedType = typeof(ClassicNearestNeighbor)},
                                 new{ ModelString="fixed:class=10", ExpectedType = typeof(ClassicFixedClassifier)},
                                 new{ ModelString="naivebayes", ExpectedType = typeof(NaiveBayesClassifier)},
                             };

            // When
            foreach(var model in models)
            {
                var classifier = ClassifierFactory.GetClassicClassifier(model.ModelString);

                // Then
                Assert.That(classifier, Is.TypeOf(model.ExpectedType), string.Format("Error getting {0}", model.ModelString));
            }
        }

        [Test]
        public void GetMultiInstanceClassifier_ValidModels()
        {
            // Given
            var models = new[]
                             {
                                 new{ ModelString="nn:k=5,distance=hausdorff", ExpectedType = typeof(MultiInstanceNearestNeighbor)},
                                 new{ ModelString="cnn:distance=hausdorff", ExpectedType = typeof(CitationNearestNeighbor)},
                                 new{ ModelString="fixed:class=5", ExpectedType = typeof(MultiInstanceFixedClassifier)}
                             };

            // When
            foreach (var model in models)
            {
                var classifier = ClassifierFactory.GetMultiInstanceClassifier(model.ModelString);

                // Then
                Assert.That(classifier, Is.TypeOf(model.ExpectedType), string.Format("Error getting {0}", model.ModelString));
            }
        }
    }
}