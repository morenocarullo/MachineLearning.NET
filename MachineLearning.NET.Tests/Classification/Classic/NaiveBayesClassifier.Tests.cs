using System.Collections.Generic;
using MachineLearning.NET.Classification;
using MachineLearning.NET.Data.Patterns;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Classification.Classic
{
    [TestFixture]
    public class NaiveBayesClassifier_Tests : BaseClassifierTestFixture
    {
        private const int UnknownClassID = -1;
        private IClassifier<SparsePattern> m_classifier;

        [SetUp]
        public void SetUp()
        {
            m_classifier = ClassifierFactory.GetClassicClassifier("naivebayes");
        }

        [Test]
        public void PredictWinner_NotTrained_ReturnUnknownClass()
        {
            // Given
            var testPattern = new SparsePattern(new Dictionary<int, float> { { 1, 1.0f }, { 2, 0.85f } }, 1);

            // When
            var winningClass = m_classifier.PredictWinner(testPattern);

            // Then
            Assert.That(winningClass, Is.EqualTo(UnknownClassID));
        }

        [Test]
        public void PredictWinner_UnseenFeatures_ReturnUnknownClass()
        {
            // Given
            const int classOne = 1;
            const int classTwo = 2;
            var patterns = new[]
                               {
                                   new SparsePattern(new Dictionary<int, float> {{1,1.0f}, {2,1.0f} }, classOne),
                                   new SparsePattern(new Dictionary<int, float> {{3,1.0f}, {4,1.0f}}, classTwo)
                               };
            var testPattern = new SparsePattern(new Dictionary<int, float> { { 7, 2.0f }, { 8, 4.0f } }, classTwo);

            // When
            m_classifier.Train(patterns);
            var winningClass = m_classifier.PredictWinner(testPattern);

            // Then
            Assert.That(winningClass, Is.EqualTo(UnknownClassID));
        }

        [Test]
        public void PredictWinner_EqualProbability()
        {
            // Given
            const int classOne = 1;
            const int classTwo = 2;
            var patterns = new[]
                               {
                                   new SparsePattern(new Dictionary<int, float> {{1,1.0f}, {2,0.85f} }, classOne),
                                   new SparsePattern(new Dictionary<int, float> {{1,1.0f}, {2,0.85f}}, classTwo)
                               };
            var testPattern = new SparsePattern(new Dictionary<int, float> { { 1, 1.0f }, { 2, 0.85f } }, classOne);

            // When
            m_classifier.Train(patterns);
            var winningClass = m_classifier.PredictWinner(testPattern);

            // Then
            Assert.That(winningClass, Is.EqualTo(classOne));
        }

        [Test]
        public void PredictWinner_ClassTwoWins()
        {
            // Given
            const int classOne = 1;
            const int classTwo = 2;
            var patterns = new[]
                               {
                                   new SparsePattern(new Dictionary<int, float> {{1,1.0f}, {2,1.0f} }, classOne),
                                   new SparsePattern(new Dictionary<int, float> {{3,1.0f}, {4,1.0f}}, classTwo)
                               };
            var testPattern = new SparsePattern(new Dictionary<int, float> { {3, 2.0f }, {4, 4.0f } }, classTwo);

            // When
            m_classifier.Train(patterns);
            var winningClass = m_classifier.PredictWinner(testPattern);

            // Then
            Assert.That(winningClass, Is.EqualTo(classTwo));
        }
    }
}