using MachineLearning.NET.RuleSystem;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.RuleSystem
{
    [TestFixture]
	public class RuleSetFactory_Test
	{
        [Test]
        public void BuildRule_EqualTo()
        {
            var ruleSet = RuleSetFactory.BuildRule("aaa=1.0");

            Assert.That(ruleSet.Count, Is.EqualTo(1));
            Assert.That(ruleSet[0], Is.EqualTo(new EqualTo("aaa", 1.0f)));
        }

        [Test]
        public void BuildRule_Greater_Less()
        {
            var ruleSet = RuleSetFactory.BuildRule("phrase_count>1 ^ length<10");

            Assert.That(ruleSet.Count, Is.EqualTo(2));
            Assert.That(ruleSet[0], Is.EqualTo(new GreaterThan("phrase_count", 1.0f)));
            Assert.That(ruleSet[1], Is.EqualTo(new LessThan("length", 10.0f)));
        }

        [Test]
        public void BuildRule_NullString()
        {
            // Given
            const string nullInputRule = null;
            var emptyInputRule = string.Empty;

            // When
            var ruleSetForNull = RuleSetFactory.BuildRule(nullInputRule);
            var ruleSetForEmpty = RuleSetFactory.BuildRule(emptyInputRule);

            // Then
            Assert.That(ruleSetForNull, Is.Not.Null);
            Assert.That(ruleSetForNull.Count, Is.EqualTo(0));
            Assert.That(ruleSetForEmpty, Is.Not.Null);
            Assert.That(ruleSetForEmpty.Count, Is.EqualTo(0));
        }
	}
}
