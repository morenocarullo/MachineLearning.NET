using NUnit.Framework;

namespace MachineLearning.NET.Tests.Classification.Classic
{
    [TestFixture]
    public class RBFNClassifier_Tests : BaseClassifierTestFixture
    {
        [Test]
        public void Train_SimpleCase_PseudoinverseOptimization()
        {
            Train_SimpleCase("rbfn:distance=euclidean,M=3,sigma=1,optimizer=pi");
        }

        [Test]
        public void Train_SimpleCase_IterativeOptimization()
        {
            Train_SimpleCase("rbfn:distance=euclidean,M=3,sigma=1,optimizer=iterative");
        }

        [Test]
        public void Train_Test_Iris_FixedSigma()
        {
            Train_Test_Iris("rbfn:distance=euclidean,M=10,sigma=1,optimizer=pi");
        }

        [Test]
        public void Train_Test_Iris_PMeanSigma()
        {
            Train_Test_Iris("rbfn:distance=euclidean,M=10,sigma=p5,optimizer=pi");
        }
    }
}
