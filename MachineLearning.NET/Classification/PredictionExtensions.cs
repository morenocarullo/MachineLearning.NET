using System.Collections.Generic;
using System.Linq;

namespace MachineLearning.NET.Classification
{
    public static class PredictionExtensions
    {
        public static IDictionary<int,float> ToProbabilities(this IDictionary<int,float> prediction)
        {
            var sum = prediction.Select(i => i.Value).Sum();

            return (from p in prediction select new {Class = p.Key, Probability = p.Value/sum}).ToDictionary(i => i.Class, i => i.Probability);
        }
    }
}
