using System;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Data.Distances
{
    [Distance(Name = "minhausdorff")]
    internal class SparseBagMinHausdorff : IDistance<SparseBag>
    {
        public double Distance(SparseBag b1, SparseBag b2)
        {
            return Math.Max(H(b1, b2), H(b2, b1));
        }

        private static double H(SparseBag b1, SparseBag b2)
        {
            var h = double.MaxValue;
            foreach (var istance1 in b1.Istances)
            {
                var shortest = double.MaxValue;
                foreach (var istance2 in b2.Istances)
                {
                    var d = Euclidean.SquaredDistance(istance1, istance2);
                    if (d < shortest)
                    {
                        shortest = d;
                    }
                }
                if (shortest < h)
                {
                    h = shortest;
                }
            }

            return Math.Sqrt(h);
        }
    }
}
