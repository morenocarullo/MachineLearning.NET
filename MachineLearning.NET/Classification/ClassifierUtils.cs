using System;
using MachineLearning.NET.Data.IO;
using MachineLearning.NET.Data.Patterns;
using MachineLearning.NET.Evaluation;

namespace MachineLearning.NET.Classification
{
    /// <summary>
    /// This utility class has utility methods applicable to IClassifier instances.
    /// </summary>
    /// <typeparam name="P"></typeparam>
    public static class ClassifierUtils<P>
        where P:class,IGenericPattern
    {
        /// <summary>
        /// Test a classifier with a given TeS with a given set of well-known metrics.
        /// </summary>
        /// <param name="classifier"></param>
        /// <param name="testFilePath"></param>
        public static ComposedQualityEvaluator<int> Test(IClassifier<P> classifier, string testFilePath)
        {
            using (var sdr = DatasetReaderFactory.GetDatasetReader<P>(testFilePath))
            using (var misclassifiedEvaluator = new MisclassifiedEvaluator(testFilePath + ".classified"))
            {
                // Evaluators
                var oaEvaluator = new OAEvaluator<int>();
                var qualityEvaluator = new ComposedQualityEvaluator<int>(
                new IQualityEvaluator<int>[]
                    {
                        oaEvaluator,
                        misclassifiedEvaluator
                    });

                // Evaluate
                classifier.Test(sdr.ToEnumerable(), qualityEvaluator);
                foreach (var metric in qualityEvaluator.GetMetrics())
                {
                    Console.WriteLine("{0} = {1}", metric.Key, metric.Value);
                }

                Console.WriteLine();

                return qualityEvaluator;
            }
        }
    }
}
