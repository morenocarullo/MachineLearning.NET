using NUnit.Framework;

namespace MachineLearning.NET.Tests.Classification.Classic
{
    [TestFixture]
    public class SVMClassifier_Tests : BaseClassifierTestFixture
    {
        [Test]
        public void Train_SimpleCase()
        {
            Train_SimpleCase("svm");
        }

        [Test]
        public void Train_Test_Iris()
        {
            Train_Test_Iris("svm");
        }
    }
}
