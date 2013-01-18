using NUnit.Framework;

namespace MachineLearning.NET.Tests.Classification.Classic
{
    [TestFixture]
    public class MLPClassifier_Test : BaseClassifierTestFixture
    {
        [Test]
        public void Train_SimpleCase()
        {
            Train_SimpleCase("mlp");
        }
    }
}