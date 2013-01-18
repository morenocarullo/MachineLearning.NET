using System;
using System.Collections.Generic;

namespace MachineLearning.NET.Data.Distances
{
    /// <summary>
    /// This class is a base implementation of Euclidean is provided.
    /// This is used in more complex distances to remove the duplication of the 'Euclidean distance' concept.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    internal static class Euclidean
    {
        public static double Distance(IDictionary<int,float> p1, IDictionary<int,float> p2)
        {
            return Math.Sqrt(SquaredDistance(p1,p2));
        }

        public static double SquaredDistance(IDictionary<int, float> p1, IDictionary<int, float> p2)
        {
            var distance = 0.0;
            foreach (var feature in p1)
            {
                if (p2.ContainsKey(feature.Key))
                    distance += Math.Pow(feature.Value - p2[feature.Key], 2);
            }
            return distance;
        }
    }
}
