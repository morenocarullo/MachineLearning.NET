using MachineLearning.NET.Evaluation;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Evaluation
{
    [TestFixture]
    public class OAEvaluator_Tests
    {
        [Test]
        public void AddData_Perfect()
        {
            // Given
            var evaluator = new OAEvaluator<int>();

            // When
            evaluator.AddData(1, 1);
            evaluator.AddData(2, 2);
            evaluator.AddData(3, 3);
            var precision = evaluator.GetValue();

            // Then
            Assert.That(precision, Is.EqualTo(1.0));
        }

        [Test]
        public void AddData_Half()
        {
            // Given
            var evaluator = new OAEvaluator<int>();

            // When
            evaluator.AddData(1, 1);
            evaluator.AddData(2, 2);
            evaluator.AddData(3, 2);
            evaluator.AddData(4, 1);
            var precision = evaluator.GetValue();

            // Then
            Assert.That(precision, Is.EqualTo(0.5));
        }

        [Test]
        public void AddData_ZeroPrecision()
        {
            // Given
            var evaluator = new OAEvaluator<int>();

            // When
            evaluator.AddData(1, 4);
            evaluator.AddData(2, 3);
            evaluator.AddData(3, 2);
            evaluator.AddData(4, 1);
            var precision = evaluator.GetValue();

            // Then
            Assert.That(precision, Is.EqualTo(0.0));
        }
    }
}