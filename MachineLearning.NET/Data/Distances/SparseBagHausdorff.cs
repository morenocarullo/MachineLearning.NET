//
// $Id$
//

using System;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Data.Distances
{
    /// <summary>
    /// In [Wang and Zucker, 2000], the minimum Hausdorff distance
    /// was used as the bag-level distance metric, defined as the shortest distance between any two
    /// instances from each bag.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2009/12/22</date>
    [Distance(Name="hausdorff")]
    internal class SparseBagHausdorff : IDistance<SparseBag>
    {
        public double Distance(SparseBag b1, SparseBag b2)
        {
            return Math.Max(H(b1, b2), H(b2, b1));
        }

        private static double H(SparseBag b1, SparseBag b2)
        {
            var h = 0.0;
            foreach(var istance1 in b1.Istances)
            {
                var shortest = double.MaxValue;
                foreach(var istance2 in b2.Istances)
                {
                    var d = Euclidean.SquaredDistance(istance1, istance2);
                    if( d < shortest)
                    {
                        shortest = d;
                    }
                }
                if( shortest > h)
                {
                    h = shortest;
                }
            }

            return Math.Sqrt(h);
        }
    }
}