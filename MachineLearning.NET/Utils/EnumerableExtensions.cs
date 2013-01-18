//
// $Id$
//

using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearning.NET.Utils
{
    /// <summary>
    /// Implements Ruby's each() method.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2010/01/08</date>
    public static class EnumerableExtensions
    {
        public static void Each<P>(this IEnumerable<P> enumerable, Action<P> action)
        {
            foreach(var element in enumerable)
            {
                action.Invoke(element);
            }
        }

        public static bool IsAlmostEqualTo<T>(this IEnumerable<T> one, IEnumerable<T> another)
        {
            if (one.Count() != another.Count()) return false;

            foreach (var element in one)
            {
                if (!another.Contains(element)) return false;
            }
            return true;
        }
    }
}