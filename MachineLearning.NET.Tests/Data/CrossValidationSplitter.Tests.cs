using System.Collections.Generic;
using MachineLearning.NET.Data;
using NUnit.Framework;
using System.Linq;

namespace MachineLearning.NET.Tests.Data
{
    [TestFixture]
	public class CrossValidationSplitter_Tests
    {
        [Test]
        public void GetSplits_Int32_MiddlePart()
        {
            // Given
            var patterns = new List<int> {1, 2, 3, 4, 5, 6};
            var crossValidator = new CrossValidationSplitter<int>(3);

            // When
            IEnumerable<int> trs;
            IEnumerable<int> tes;
            crossValidator.GetSplits(patterns, out trs, out tes, 2);

            // Then
            Assert.That(trs.Count(), Is.EqualTo(4));
            Assert.That(trs, Has.Member(1));
            Assert.That(trs, Has.Member(2));
            Assert.That(trs, Has.Member(5));
            Assert.That(trs, Has.Member(6));
            Assert.That(tes.Count(), Is.EqualTo(2));
            Assert.That(tes, Has.Member(3));
            Assert.That(tes, Has.Member(4));
        }

        [Test]
        public void GetSplits_Int32_LastPart()
        {
            // Given
            var patterns = new List<int> { 1, 2, 3, 4, 5, 6 };
            var crossValidator = new CrossValidationSplitter<int>(3);

            // When
            IEnumerable<int> trs;
            IEnumerable<int> tes;
            crossValidator.GetSplits(patterns, out trs, out tes, 3);

            // Then
            Assert.That(trs.Count(), Is.EqualTo(4));
            Assert.That(trs, Has.Member(1));
            Assert.That(trs, Has.Member(2));
            Assert.That(trs, Has.Member(3));
            Assert.That(trs, Has.Member(4));
            Assert.That(tes.Count(), Is.EqualTo(2));
            Assert.That(tes, Has.Member(5));
            Assert.That(tes, Has.Member(6));
        }

        [Test]
        public void GetSplits_Int32_FirstPart()
        {
            // Given
            var patterns = new List<int> { 1, 2, 3, 4, 5, 6 };
            var crossValidator = new CrossValidationSplitter<int>(3);

            // When
            IEnumerable<int> trs;
            IEnumerable<int> tes;
            crossValidator.GetSplits(patterns, out trs, out tes, 1);

            // Then
            Assert.That(trs.Count(), Is.EqualTo(4));
            Assert.That(trs, Has.Member(5));
            Assert.That(trs, Has.Member(6));
            Assert.That(trs, Has.Member(3));
            Assert.That(trs, Has.Member(4));
            Assert.That(tes.Count(), Is.EqualTo(2));
            Assert.That(tes, Has.Member(1));
            Assert.That(tes, Has.Member(2));
        }
    }
}