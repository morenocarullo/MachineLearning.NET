using MachineLearning.NET.Data.Distances.EMD;
using NUnit.Framework;

namespace MachineLearning.NET.Tests.Data.Distances
{
    [TestFixture]
	public class EMD_Tests
    {
        [Test]
        public virtual void TestEMD_standard1()
        {
            var e = new EMD();
            var costMatrix = new []{
                                       new []{109.927246, 97.283089, 352.900848},
                                       new []{211.955185, 195.971939, 348.094818},
                                       new []{244.180267, 115.429634, 254.909790},
                                       new []{141.435501, 52.000000, 334.752136}
                                   };

            var s1 = new Signature(new[] { 0.4, 0.3, 0.2, 0.1 });
            var s2 = new Signature(new[] { 0.5, 0.3, 0.2 });

            double ris = e.Emd(s1, s2, costMatrix, null, 0, null);
            System.Console.Out.WriteLine(ris);
            var exp = 16054;

            Assert.AreEqual(exp, (int)(ris * 100));
        }

        [Test]
        public void TestEMD_standard2()
        {
            var e = new EMD();
            var Dist = new[]
                           {
                               new[] {51.029404, 237.354172, 237.953781},
                               new[] {87.624199, 212.002365, 180.305298},
                               new[] {89.448311, 157.556335, 254.517197},
                               new[] {237.404724, 169.717407, 121.897499}
                           };

            var s1 = new Signature(new[] { 0.2,0.6,0.4,0.1 });
            var s2 = new Signature(new[] { 0.1,0.6,0.1 });
            
            double ris = e.Emd(s1, s2, Dist, null, 0, null);
            System.Console.Out.WriteLine(ris);
            var exp = 15339;
            Assert.AreEqual(exp, (int)(ris * 100));
        }
    }
}