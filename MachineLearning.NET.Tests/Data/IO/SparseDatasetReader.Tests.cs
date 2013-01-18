using System.IO;
using System.Linq;
using System.Collections.Generic;
using MachineLearning.NET.Data.IO;
using MachineLearning.NET.Data.Patterns;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Data.IO
{
    [TestFixture]
	public class SparseDatasetReader_Test
    {
        [Test]
        public void AllElements_Twice()
        {
            // Given
            var tmpPath = Path.GetTempFileName();
            try
            {
                var patterns = new List<SparsePattern>();
                using (var dw = DatasetWriterFactory.GetDatasetWriter<SparsePattern>(tmpPath))
                {
                    for (var patternId = 0; patternId < 500; patternId++)
                    {
                        foreach (var pattern in patterns) dw.Write(pattern);
                    }
                }

                // When
                using (var dr = DatasetReaderFactory.GetDatasetReader<SparsePattern>(tmpPath))
                {
                    var elementsFirstTime = dr.ToList();
                    var elementsSecondTime = dr.ToList();

                    // Then
                    Assert.That(elementsFirstTime, Is.EquivalentTo(patterns));
                    Assert.That(elementsSecondTime, Is.EquivalentTo(patterns));
                }
            }
            finally
            {
                if (File.Exists(tmpPath))
                {
                    File.Delete(tmpPath);
                }
            }
        }

        [Test]
        public void Enumerable_Twice()
        {
            // Given
            var filePath = Path.Combine(TestConstants.TestFilesPath, "sparseDataset.txt");

            // When
            using (var dr = DatasetReaderFactory.GetDatasetReader<SparsePattern>(filePath))
            {
                var firstFirstTime = dr.First();
                var firstSecondTime = dr.First();

                Assert.That(firstFirstTime.Label, Is.EqualTo(1));
                Assert.That(firstFirstTime.Features.Count, Is.EqualTo(2));
                Assert.That(firstFirstTime.Features[1], Is.EqualTo(2.0f));
                Assert.That(firstFirstTime.Features[2], Is.EqualTo(3.0f));

                Assert.That(firstSecondTime.Label, Is.EqualTo(1));
                Assert.That(firstSecondTime.Features.Count, Is.EqualTo(2));
                Assert.That(firstSecondTime.Features[1], Is.EqualTo(2.0f));
                Assert.That(firstSecondTime.Features[2], Is.EqualTo(3.0f));
            }
        }
    }
}
