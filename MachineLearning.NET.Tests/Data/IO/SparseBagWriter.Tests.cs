using System.Collections.Generic;
using System.IO;
using MachineLearning.NET.Data.IO;
using MachineLearning.NET.Data.Patterns;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Data.IO
{
    [TestFixture]
	public class SparseBagWriter_Tests
	{
        [Test]
        public void Write_Verify()
        {
            var tmpFileName = Path.GetTempFileName();
            try
            {
                // Given
                var bag1 = new SparseBag(1, "bag1", new[] {new Dictionary<int, float> {{1, 0.5f}}});
                var bag2 = new SparseBag(1, "bag2", new[] { new Dictionary<int, float> { { 1, 0.5f } }, new Dictionary<int, float> { { 2, 0.6f }, { 3, 1.6f } } });
                var sparseBagWriter = DatasetWriterFactory.GetDatasetWriter<SparseBag>(tmpFileName);

                // When
                sparseBagWriter.Write(bag1);
                sparseBagWriter.Write(bag2);
                sparseBagWriter.Dispose();
                var sparseBagReader = DatasetReaderFactory.GetDatasetReader<SparseBag>(tmpFileName);
                var readBag1 = sparseBagReader.Next();
                var readBag2 = sparseBagReader.Next();
                var readBag3 = sparseBagReader.Next();

                // Then
                Assert.That(readBag1, Is.EqualTo(bag1));
                Assert.That(readBag2, Is.EqualTo(bag2));
                Assert.That(readBag3, Is.Null);
            }
            finally
            {
                if(File.Exists(tmpFileName))
                {
                    File.Delete(tmpFileName);
                }
            }
        }
	}
}
