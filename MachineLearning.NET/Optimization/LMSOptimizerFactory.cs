using System;
using MachineLearning.NET.Utils;

namespace MachineLearning.NET.Optimization
{
    public static class LMSOptimizerFactory
    {
        /// <summary>
        /// Get a LMS optimizer from configuration string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ILMSOptimizer GetOptimizer(string configuration)
        {
            var parts = configuration.Split(':');
            var kind = parts[0];

            var config = configuration.GetParamsFromConfigString();
            switch (kind)
            {
                case "pi":
                    return new PseudoinverseOptimizer(config);
                case "octave":
                    return new OctavePseudoinverseOptimizer(config);
                case "iterative":
                    return new WidrowHoffOptimizer(config);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}