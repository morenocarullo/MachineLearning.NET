using MachineLearning.NET.Evaluation;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Evaluation
{
    [TestFixture]
	public class RecallEvaluator_Tests
    {
        [Test]
        public void AddData_SingleObject_PerfectRecall()
        {
            // Given
            var recallEvaluator = new RecallEvaluator<int>();

            // When
            recallEvaluator.AddData(1, 1);
            recallEvaluator.AddData(2, 2);
            recallEvaluator.AddData(3, 3);
            var precision = recallEvaluator.GetValue();

            // Then
            Assert.That(precision, Is.EqualTo(1.0));
        }

        [Test]
        public void AddData_SingleObject_HalfRecall()
        {
            // Given
            var recallEvaluator = new RecallEvaluator<int>();

            // When
            recallEvaluator.AddData(1, 1);
            recallEvaluator.AddData(2, 2);
            recallEvaluator.AddData(3, 2);
            recallEvaluator.AddData(4, 1);
            var precision = recallEvaluator.GetValue();

            // Then
            Assert.That(precision, Is.EqualTo(0.5));
        }

        [Test]
        public void AddData_SingleObject_ZeroRecall()
        {
            // Given
            var recallEvaluator = new RecallEvaluator<int>();

            // When
            recallEvaluator.AddData(1, 4);
            recallEvaluator.AddData(2, 3);
            recallEvaluator.AddData(3, 2);
            recallEvaluator.AddData(4, 1);
            var precision = recallEvaluator.GetValue();

            // Then
            Assert.That(precision, Is.EqualTo(0.0));
        }
    }
}
