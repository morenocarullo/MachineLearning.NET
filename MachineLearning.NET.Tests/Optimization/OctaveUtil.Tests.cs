using MachineLearning.NET.Optimization;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Optimization
{
    [TestFixture]
	public class OctaveUtil_Tests
    {
        private const string TestFilesPath = TestConstants.TestFilesPath;

        [Test]
        public void LoadOctaveMatrix_Big_IsLoaded()
        {
            const string filePath = TestFilesPath+"/octave_test_matrix.mat";

            var matrix = OctaveUtil.LoadOctaveMatrix(filePath, "r");

            Assert.That(matrix.Rows, Is.EqualTo(125));
            Assert.That(matrix.Columns, Is.EqualTo(99));
        }

        [Test]
        public void LoadOctaveMatrix_Correct()
        {
            const string filePath = TestFilesPath + "/octave_simple_matrix.mat";

            var matrix = OctaveUtil.LoadOctaveMatrix(filePath, "simple");

            Assert.That(matrix.Rows, Is.EqualTo(2));
            Assert.That(matrix.Columns, Is.EqualTo(2));
            Assert.That(matrix[0, 0], Is.EqualTo(1));
            Assert.That(matrix[0, 1], Is.EqualTo(2));
            Assert.That(matrix[1, 0], Is.EqualTo(3));
            Assert.That(matrix[1, 1], Is.EqualTo(4));
        }
    }
}