using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearning.NET.Evaluation
{
    public class RecallEvaluator<T> : IQualityEvaluator<T>
        where T : IEquatable<T>
    {
        private int m_allFound;
        private int m_intersectionCount;

        public string MetricName
        {
            get { return "R"; }
        }

        public void AddData(HashSet<T> expected, HashSet<T> found)
        {
            m_intersectionCount += expected.Intersect(found).Count();
            m_allFound += expected.Count;
        }

        public void AddData(T expected, T found)
        {
            if (expected.Equals(found)) m_intersectionCount++;
            m_allFound++;
        }

        public void AddDataBatch(int correctlyFound, int expectedCount)
        {
            m_intersectionCount += correctlyFound;
            m_allFound += expectedCount;
        }

        public double GetValue()
        {
            return m_allFound > 0 ? m_intersectionCount / (float)m_allFound : 0;
        }
    }
}