using MachineLearning.NET.Mapack;
using MachineLearning.NET.Optimization;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Optimization
{
    [TestFixture, Ignore("Execute this manually since it requires GNU Octave")]
	public class OctavePseudoinverseOptimizer_Tests
    {
        [Test]
        public void Optimize_SmallMatrix()
        {
            // Given
            var x = Matrix.Diagonal(100,50,1.0);
            var t = Matrix.Diagonal(100, 50, 1.0);
            var optimizer = LMSOptimizerFactory.GetOptimizer("octave");

            // When
            var optimized = optimizer.Optimize(x, t);

            // Then
            Assert.That(optimized.Rows, Is.EqualTo(50));
            Assert.That(optimized.Columns, Is.EqualTo(50));
        }
    }
}
