using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearning.NET.Evaluation
{
    /// <summary>
    /// Permits to run a collection of quality evaluation routines.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ComposedQualityEvaluator<T> : IQualityEvaluator<T>
        where T : IEquatable<T>
    {
        private readonly IEnumerable<IQualityEvaluator<T>> m_qualityEvaluators;

        public ComposedQualityEvaluator(IEnumerable<IQualityEvaluator<T>> qualityEvaluators)
        {
            m_qualityEvaluators = qualityEvaluators;
        }

        public string MetricName
        {
            get {
                return string.Join(", ", (from e in m_qualityEvaluators select e.MetricName).ToArray());
            }
        }

        public void AddData(HashSet<T> expected, HashSet<T> found)
        {
            foreach (var evaluator in m_qualityEvaluators)
            {
                evaluator.AddData(expected, found);
            }
        }

        public void AddData(T expected, T found)
        {
            foreach (var evaluator in m_qualityEvaluators)
            {
                evaluator.AddData(expected, found);
            }
        }

        public double GetValue()
        {
            return -1;
        }

        public IDictionary<string,double> GetMetrics()
        {
            var metrics = new Dictionary<string, double>();
            foreach(var evaluator in m_qualityEvaluators)
            {
                metrics.Add(evaluator.MetricName, evaluator.GetValue());
            }
            return metrics;
        }
    }
}