using System.Collections.Generic;
using MachineLearning.NET.RuleSystem;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.RuleSystem
{
    [TestFixture]
	public class RuleSet_Tests
    {
        [Test]
        public void EmptyRuleSet_AlwaysTrue()
        {
            var ruleSet = new RuleSet(new Rule[0]);

            var propertiesForObject = new Dictionary<object, object> { { "phrase_count", 2 }, { "length", 10 } };

            Assert.That(ruleSet.VerifiedBy(propertiesForObject));
        }

        [Test]
        public void Equality_False()
        {
            var ruleSet = new RuleSet(new[] { new EqualTo("ciao", 2) });

            var propertiesForObject = new Dictionary<object, object> { { "ciao", 1 }, { "length", 10 } };

            Assert.That(ruleSet.VerifiedBy(propertiesForObject), Is.False);
        }

        [Test]
        public void Equality_True()
        {
            var ruleSet = new RuleSet(new[] { new EqualTo("ciao", 1) });

            var propertiesForObject = new Dictionary<object, object> { { "ciao", 1 }, { "length", 10 } };

            Assert.That(ruleSet.VerifiedBy(propertiesForObject), Is.True);
        }

        [Test]
        public void Verifies_SimpleFalseRule()
        {
            var propertiesForObject = new Dictionary<object, object> { { "phrase_count", 1 } };

            var ruleSet = new RuleSet (new[]{new GreaterThan("phrase_count", 3)});

            Assert.That(ruleSet.VerifiedBy(propertiesForObject), Is.False);
        }

        [Test]
        public void Verifies_SimpleTrueRules()
        {
            var propertiesForObject = new Dictionary<object, object> { { "phrase_count", 2 }, { "length", 10 } };

            var ruleSet = new RuleSet(new Rule[] { new GreaterThan("phrase_count", 1), new LessThan("length", 15) });

            Assert.That(ruleSet.VerifiedBy(propertiesForObject), Is.True);
        }
    }
}