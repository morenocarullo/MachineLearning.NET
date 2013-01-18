using System;
using System.Linq;
using System.Reflection;
using MachineLearning.NET.Utils;

namespace MachineLearning.NET.Data.Distances
{
    /// <summary>
    /// This factory permits to obtain a distance metric for a desired type P
    /// with a given name.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2009/12/22</date>
    public static class DistanceFactory
    {
        public static IDistance<P> GetDistanceMetric<P>(string metricName)
        {
            var searchedDistance =
                (from t in Assembly.GetExecutingAssembly().GetTypes()
                 let attributes = t.GetCustomAttributes(typeof(DistanceAttribute), false)
                 where
                     t.GetInterfaces().Contains(typeof(IDistance<P>)) &&
                     attributes != null &&
                     attributes.Any(o => ((DistanceAttribute)o).Name == metricName)
                 select t).FirstOrDefault();

            if (searchedDistance == null)
            {
                throw new ArgumentException(string.Format("The specified metric {0} doesn't exist for pattern {1}", metricName, typeof(P).Name));
            }

            return InstanceBuilder<IDistance<P>>.BuildInstance<IDistance<P>>(searchedDistance);
        }
    }
}