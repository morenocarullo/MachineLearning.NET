//
// $Id: Tool.cs 21954 2010-11-03 08:27:35Z xpuser $
//

using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MachineLearning.NET.Classification;
using MachineLearning.NET.Data;
using MachineLearning.NET.Data.IO;
using MachineLearning.NET.Data.Patterns;
using MachineLearning.NET.Evaluation;
using MachineLearning.NET.Utils;

namespace MachineLearning.NET
{
    /// <summary>
    /// This program is intended for the train and test of machine learning algorithm within
    /// the ArteML library.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    public static class Tool
    {
        private static readonly ILog s_log = LogManager.GetLogger(typeof(Tool));

        public enum MachineLearningMode
        {
            Classic, MultiInstance
        }

        public static void TrainAndTest(string modelName, MachineLearningMode mode, string trsPath, string tesPath)
        {
            switch (mode)
            {
                case MachineLearningMode.Classic:
                    {
                        Console.WriteLine("*** Classic mode.");
                        var classifier = ClassifierFactory.GetClassicClassifier(modelName);
                        TrainAndTest(classifier, trsPath, tesPath, 0);
                    }
                    break;
                case MachineLearningMode.MultiInstance:
                    {
                        Console.WriteLine("*** Multi-instance mode.");
                        var classifier = ClassifierFactory.GetMultiInstanceClassifier(modelName);
                        TrainAndTest(classifier, trsPath, tesPath, 0);
                    }
                    break;
            }
        }

        public static void CrossValidate(string modelName, MachineLearningMode mode, string datasetPath, int cvFolding)
        {
            switch (mode)
            {
                case MachineLearningMode.Classic:
                    {
                        Console.WriteLine("*** Classic mode.");
                        var classifier = ClassifierFactory.GetClassicClassifier(modelName);
                        TrainAndTest(classifier, datasetPath, null, cvFolding);
                    }
                    break;
                case MachineLearningMode.MultiInstance:
                    {
                        Console.WriteLine("*** Multi-instance mode.");
                        var classifier = ClassifierFactory.GetMultiInstanceClassifier(modelName);
                        TrainAndTest(classifier, datasetPath, null, cvFolding);
                    }
                    break;
            }
        }

        /// <summary>
        /// Train and test with the data. If necessary, use cross-validation to evaluate the training set.
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <param name="classifier"></param>
        /// <param name="trainFile"></param>
        /// <param name="testFile"></param>
        /// <param name="cv"></param>
        static void TrainAndTest<P>(IClassifier<P> classifier, string trainFile, string testFile, int cv)
            where P : class, IGenericPattern
        {
            s_log.InfoFormat("Start");

            if(!string.IsNullOrEmpty(testFile))
            {
                Console.WriteLine("*** Standard TrS/TeS learning.");
                classifier.Train(DatasetReaderFactory.GetDatasetReader<P>(trainFile));
                ClassifierUtils<P>.Test(classifier, testFile);
            }
            else
            {
                // Cross Validation
                if(cv == 0)
                {
                    throw new ArgumentException("It should be > 0","cv");
                }
                Console.WriteLine("*** Performing {0}-fold Cross Validation", cv);

                var data = DatasetReaderFactory.GetDatasetReader<P>(trainFile).ToList();
                var oaEvaluator = new OAEvaluator<int>();
                var crossValidationSplitter = new CrossValidationSplitter<P>(cv);

                Console.WriteLine("*** TrS has {0} patterns.", data.Count());

                Enumerable.Range(1, cv).Each(
                    cvPart =>
                        {
                            Console.WriteLine("*** Performing {0} part of CV.", (object) cvPart);

                            IEnumerable<P> trs;
                            IEnumerable<P> tes;
                            crossValidationSplitter.GetSplits(data, out trs, out tes, cvPart);

                            // Write CV-TrS
                            var cvTrsPathFile = trainFile + ".cv-" + cvPart;
                            using (var dw = DatasetWriterFactory.GetDatasetWriter<P>(cvTrsPathFile))
                            {
                                trs.Each(dw.Write);
                            }

                            // Train
                            Console.WriteLine("*** Training ({0} patterns)...", trs.Count());
                            classifier.Forget();
                            classifier.Train(DatasetReaderFactory.GetDatasetReader<P>(cvTrsPathFile));

                            // Test
                            Console.WriteLine("*** Testing ({0} patterns)...", tes.Count());
                            tes.Each(p =>
                                         {
                                             var predictedLabel = classifier.PredictWinner(p);
                                             lock (oaEvaluator)
                                             {
                                                 oaEvaluator.AddData(p.Label, predictedLabel);
                                             }
                                         });
                        });

                Console.WriteLine("*** {0}-fold CrossValidation OA result = {1}", cv, oaEvaluator.GetValue());
            }

            s_log.InfoFormat("Finished");
        }
    }
}