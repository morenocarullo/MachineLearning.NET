using System.IO;
using System.Linq;
using MachineLearning.NET.Data.IO;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Data.IO
{
    [TestFixture]
	public class SparseBagReader_Tests
	{
        private string m_datasetFilePath = Path.Combine(TestConstants.TestFilesPath, "sparseBags/musk1.data");

        [Test]
        public void Next_MuskDataset()
        {
            // Given

            // When
            using (var sparseBagReader = new SparseBagReader(m_datasetFilePath))
            {
                var firstBag = sparseBagReader.Next();
                var secondBag = sparseBagReader.Next();

                // Then
                Assert.That(firstBag, Is.Not.Null);
                Assert.That(firstBag.BagName, Is.EqualTo("MUSK-188"));
                Assert.That(firstBag.Label, Is.EqualTo(1));
                Assert.That(firstBag.Istances.Length, Is.EqualTo(4));
                Assert.That(firstBag.Istances[0][3], Is.EqualTo(-109.0f));
                Assert.That(firstBag.Istances[0][1], Is.EqualTo(42.0f));
                Assert.That(secondBag.BagName, Is.EqualTo("MUSK-190"));
                Assert.That(secondBag.Istances.Length, Is.EqualTo(4));
            }
        }

        [Test]
        public void Enumerable_Twice()
        {
            // Given

            // When
            using (var sparseBagReader = new SparseBagReader(m_datasetFilePath))
            {
                var firstBagFirstTime = sparseBagReader.First();
                var firstBagSecondTime = sparseBagReader.First();

                // Then
                Assert.That(firstBagFirstTime, Is.Not.Null);
                Assert.That(firstBagFirstTime.BagName, Is.EqualTo("MUSK-188"));
                Assert.That(firstBagFirstTime.Label, Is.EqualTo(1));
                Assert.That(firstBagFirstTime.Istances.Length, Is.EqualTo(4));
                Assert.That(firstBagFirstTime.Istances[0][3], Is.EqualTo(-109.0f));
                Assert.That(firstBagFirstTime.Istances[0][1], Is.EqualTo(42.0f));

                Assert.That(firstBagSecondTime, Is.Not.Null);
                Assert.That(firstBagSecondTime.BagName, Is.EqualTo("MUSK-188"));
                Assert.That(firstBagSecondTime.Label, Is.EqualTo(1));
                Assert.That(firstBagSecondTime.Istances.Length, Is.EqualTo(4));
                Assert.That(firstBagSecondTime.Istances[0][3], Is.EqualTo(-109.0f));
                Assert.That(firstBagSecondTime.Istances[0][1], Is.EqualTo(42.0f));
            }
        }
	}
}
