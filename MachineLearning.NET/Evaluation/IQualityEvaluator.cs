using System;
using System.Collections.Generic;

namespace MachineLearning.NET.Evaluation
{
    /// <summary>
    /// A quality evaluator records a metric for a given algorithm instance.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2009/12/07</date>
    /// <typeparam name="T"></typeparam>
    public interface IQualityEvaluator<T>
        where T : IEquatable<T>
    {
        /// <summary>
        /// The name of the evaluation metric.
        /// </summary>
        string MetricName { get;}

        /// <summary>
        /// Add a single evaluation data specifying the two sets.
        /// </summary>
        /// <param name="expected">the expected set</param>
        /// <param name="found">the found set</param>
        void AddData(HashSet<T> expected, HashSet<T> found);

        /// <summary>
        /// Add a single evaluation data specifying two singleton sets.
        /// </summary>
        /// <param name="expected">the expected value</param>
        /// <param name="found">the found value</param>
        void AddData(T expected, T found);

        /// <summary>
        /// The the value of the metric.
        /// </summary>
        /// <returns>the value of the metric</returns>
        double GetValue();
    }
}