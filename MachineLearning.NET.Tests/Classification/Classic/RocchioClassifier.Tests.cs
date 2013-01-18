using NUnit.Framework;

namespace MachineLearning.NET.Tests.Classification.Classic
{
    [TestFixture]
	public class RocchioClassifier_Tests : BaseClassifierTestFixture
	{
        [Test]
        public void Train_SimpleCase()
        {
            Train_SimpleCase("rocchio");
        }
	}
}
