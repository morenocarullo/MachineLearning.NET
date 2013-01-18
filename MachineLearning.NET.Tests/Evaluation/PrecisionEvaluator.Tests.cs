using MachineLearning.NET.Evaluation;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Evaluation
{
    [TestFixture]
	public class PrecisionEvaluator_Tests
    {
        [Test]
        public void AddData_SingleObject_PerfectPrecision()
        {
            // Given
            var precisionEvaluator = new PrecisionEvaluator<int>();

            // When
            precisionEvaluator.AddData(1, 1);
            precisionEvaluator.AddData(2, 2);
            precisionEvaluator.AddData(3, 3);
            var precision = precisionEvaluator.GetValue();

            // Then
            Assert.That(precision, Is.EqualTo(1.0));
        }

        [Test]
        public void AddData_SingleObject_HalfPrecision()
        {
            // Given
            var precisionEvaluator = new PrecisionEvaluator<int>();

            // When
            precisionEvaluator.AddData(1, 1);
            precisionEvaluator.AddData(2, 2);
            precisionEvaluator.AddData(3, 2);
            precisionEvaluator.AddData(4, 1);
            var precision = precisionEvaluator.GetValue();

            // Then
            Assert.That(precision, Is.EqualTo(0.5));
        }

        [Test]
        public void AddData_SingleObject_ZeroPrecision()
        {
            // Given
            var precisionEvaluator = new PrecisionEvaluator<int>();

            // When
            precisionEvaluator.AddData(1, 4);
            precisionEvaluator.AddData(2, 3);
            precisionEvaluator.AddData(3, 2);
            precisionEvaluator.AddData(4, 1);
            var precision = precisionEvaluator.GetValue();

            // Then
            Assert.That(precision, Is.EqualTo(0.0));
        }

        [Test]
        public void AddDataBatch_OnePrecision()
        {
            // Given
            var precisionEvaluator = new PrecisionEvaluator<int>();

            // When
            precisionEvaluator.AddDataBatch(100, 100);
            var precision = precisionEvaluator.GetValue();

            // Then
            Assert.That(precision, Is.EqualTo(1.0));
        }

        [Test]
        public void AddDataBatch_HalfPrecision()
        {
            // Given
            var precisionEvaluator = new PrecisionEvaluator<int>();

            // When
            precisionEvaluator.AddDataBatch(50, 100);
            var precision = precisionEvaluator.GetValue();

            // Then
            Assert.That(precision, Is.EqualTo(0.5));
        }

        [Test]
        public void AddDataBatch_ZeroPrecision()
        {
            // Given
            var precisionEvaluator = new PrecisionEvaluator<int>();

            // When
            precisionEvaluator.AddDataBatch(0, 100);
            var precision = precisionEvaluator.GetValue();

            // Then
            Assert.That(precision, Is.EqualTo(0.0));
        }
    }
}
