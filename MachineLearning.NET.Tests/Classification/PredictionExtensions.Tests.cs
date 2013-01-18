using System.Collections.Generic;
using MachineLearning.NET.Classification;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Classification
{
    [TestFixture]
    public class PredictionExtensions
    {
        [Test]
        public void ToProbabilities_Empty()
        {
            // Arrange
            var prediction = new Dictionary<int, float>();
            var expectedProbabilities = new Dictionary<int, float>();

            // Act
            var probabilities = prediction.ToProbabilities();

            // Assert
            Assert.That(probabilities, Is.EqualTo(expectedProbabilities));
        }

        [Test]
        public void ToProbabilities_ToNormalize()
        {
            // Arrange
            var prediction = new Dictionary<int, float> {{1, 0.3f}, {2, 0.2f}};
            var expectedProbabilities = new Dictionary<int, float> { { 1, 0.6f }, { 2, 0.4f } };

            // Act
            var probabilities = prediction.ToProbabilities();

            // Assert
            Assert.That(probabilities, Is.EqualTo(expectedProbabilities));
        }
    }
}
