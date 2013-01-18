using System;
using System.Collections.Generic;
using System.IO;

namespace MachineLearning.NET.Evaluation
{
    public class MisclassifiedEvaluator : IDisposable, IQualityEvaluator<int>
    {
        private readonly StreamWriter m_resultsStream;
        private int m_misclassifiedCount;

        public MisclassifiedEvaluator(string resultsFilePath)
        {
            m_resultsStream = File.CreateText(resultsFilePath);
        }

        public void Dispose()
        {
            m_resultsStream.Dispose();
        }

        public string MetricName
        {
            get { return "MisclassifiedCount"; }
        }

        public void AddData(HashSet<int> expected, HashSet<int> found)
        {
        }

        public void AddData(int expected, int found)
        {
            m_resultsStream.WriteLine("{0} {1}", expected, found);
            if(expected != found)
            {
                m_misclassifiedCount++;
            }
        }

        public double GetValue()
        {
            return m_misclassifiedCount;
        }
    }
}