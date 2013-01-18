//
// $Id: OAEvaluator.cs 21954 2010-11-03 08:27:35Z xpuser $
//

using System;
using System.Collections.Generic;

namespace MachineLearning.NET.Evaluation
{
    /// <summary>
    /// A simple Overall Accuracy evaluator.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2009/12/07</date>
    /// <typeparam name="T">must have a valid Equals implementation</typeparam>
    public class OAEvaluator<T> : IQualityEvaluator<T>
        where T : IEquatable<T>
    {
        private int m_hitCount;
        private int m_totalCount;

        public string MetricName
        {
            get { return "OA"; }
        }

        public void AddData(HashSet<T> expected, HashSet<T> found)
        {
            if (expected.SetEquals(found)) m_hitCount++;
            m_totalCount++;
        }

        public void AddData(T expected, T found)
        {
            if (expected.Equals(found)) m_hitCount++;
            m_totalCount++;
        }

        public double GetValue()
        {
            return ((float) m_hitCount)/m_totalCount;
        }

        public string GetReport()
        {
            return string.Format("OA: {0:#.##} % [{1} hit, {2} total]",
                                 100 * GetValue(), m_hitCount, m_totalCount);
        }
    }
}