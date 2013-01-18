using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearning.NET.Evaluation
{
    public class PrecisionEvaluator<T>: IQualityEvaluator<T>
        where T : IEquatable<T>
    {
        private int m_foundCount;
        private int m_correctlyFound;

        public string MetricName
        {
            get { return "P"; }
        }

        public void AddData(HashSet<T> expected, HashSet<T> found)
        {
            m_correctlyFound += expected.Intersect(found).Count();
            m_foundCount += found.Count;
        }

        public void AddData(T expected, T found)
        {
            if (expected.Equals(found)) m_correctlyFound++;
            m_foundCount++;
        }

        public void AddDataBatch(int correctlyFound, int foundCount)
        {
            m_correctlyFound += correctlyFound;
            m_foundCount += foundCount;
        }

        public double GetValue()
        {
            return m_foundCount > 0 ? m_correctlyFound/(float) m_foundCount : 0;
        }
    }
}