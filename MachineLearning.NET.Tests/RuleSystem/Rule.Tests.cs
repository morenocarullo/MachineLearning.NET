using MachineLearning.NET.RuleSystem;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.RuleSystem
{
    [TestFixture]
	public class Rule_Tests
    {
        [Test]
        public void Equals_False_ByProperty()
        {
            var simpleRule1 = new GreaterThan("cieo", 10);
            var simpleRule2 = new GreaterThan("ciao", 10);

            Assert.That(simpleRule1, Is.Not.EqualTo(simpleRule2));
        }

        [Test]
        public void Equals_False_ByValue()
        {
            var simpleRule1 = new GreaterThan("ciao", 10);
            var simpleRule2 = new GreaterThan("ciao", 11);

            Assert.That(simpleRule1, Is.Not.EqualTo(simpleRule2));
        }

        [Test]
        public void Equals_True_1()
        {
            var simpleRule1 = new GreaterThan("ciao", 10);
            var simpleRule2 = new GreaterThan("ciao", 10);

            Assert.That(simpleRule1, Is.EqualTo(simpleRule2));
        }

        [Test]
        public void Equals_True_2()
        {
            var simpleRule1 = new LessThan("ciao", 10);
            var simpleRule2 = new LessThan("ciao", 10);

            Assert.That(simpleRule1, Is.EqualTo(simpleRule2));
        }
    }
}