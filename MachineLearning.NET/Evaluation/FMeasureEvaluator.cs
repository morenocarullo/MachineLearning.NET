using System;
using System.Collections.Generic;

namespace MachineLearning.NET.Evaluation
{
    public class FMeasureEvaluator<T> : IQualityEvaluator<T>
        where T : IEquatable<T>
    {
        private readonly double m_beta;
        private readonly PrecisionEvaluator<T> m_precisionEvaluator;
        private readonly RecallEvaluator<T> m_recallEvaluator;

        public FMeasureEvaluator(double beta, PrecisionEvaluator<T> precisionEvaluator, RecallEvaluator<T> recallEvaluator)
        {
            m_beta = beta;
            m_precisionEvaluator = precisionEvaluator;
            m_recallEvaluator = recallEvaluator;
        }

        public string MetricName
        {
            get { return string.Format("F_{0}", m_beta); }
        }

        public void AddData(HashSet<T> expected, HashSet<T> found)
        {            
        }

        public void AddData(T expected, T found)
        {
        }

        public double GetValue()
        {
            var precision = m_precisionEvaluator.GetValue();
            var recall = m_recallEvaluator.GetValue();
            return (precision + recall) > 0 ? 2 * precision * recall / (precision + recall) : 0;
        }
    }
}