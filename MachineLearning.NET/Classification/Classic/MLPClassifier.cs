using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using MachineLearning.NET.Data.IO;
using MachineLearning.NET.Data.Patterns;
using MachineLearning.NET.Regression.MLP;

namespace MachineLearning.NET.Classification.Classic
{
    /// <summary>
    /// An MLP Classifier usign Ignazio's Multi-Layer Perceptron implementation.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2009/11/11</date>
    [Classifier(Name="mlp")]
    public class MLPClassifier : BaseClassifier<SparsePattern>
    {
        private static readonly ILog s_log = LogManager.GetLogger(typeof (MLPClassifier));

        /// <summary>
        /// The MLP model used to predict class.
        /// </summary>
        private MLP m_model;

        /// <summary>
        /// Number of requested hidden neurons.
        /// </summary>
        private readonly int m_numOfHiddenNeurons = 50;

        /// <summary>
        /// Number of requested training epochs.
        /// </summary>
        private readonly int m_epochs = 300;

        /// <summary>
        /// Minimum classID for this problem. It is required to convert from/to SparsePattern.
        /// </summary>
        private int m_minClassID;

        public MLPClassifier(IDictionary<string, string> config)
        {
            if(config.ContainsKey("hidden"))
            {
                m_numOfHiddenNeurons = int.Parse(config["hidden"]);
            }
            if(config.ContainsKey("epochs"))
            {
                m_epochs = int.Parse(config["epochs"]);
            }
        }

        public override bool IsTrained
        {
            get
            {
                return m_model != null;
            }
        }

        public override bool HasProbabilities
        {
            get
            {
                return true;
            }
        }

        public override void Train(IEnumerable<SparsePattern> patterns)
        {
            var maxFeatID = int.MinValue;
            var maxClassID = int.MinValue;
            m_minClassID = int.MaxValue;
            int numInputs = -1;
            int numClasses = -1;

            // Compute # of features and # of classes
            s_log.DebugFormat("Computing # of features");
            foreach(var sp in patterns)
            {
                var patternMaxFeatID = (from f in sp.Features select f.Key).Max();
                maxFeatID = Math.Max(patternMaxFeatID, maxFeatID);
                maxClassID = Math.Max(maxClassID, sp.Label);
                m_minClassID = Math.Min(m_minClassID, sp.Label);
            }
            numClasses = maxClassID - m_minClassID + 1;
            numInputs = maxFeatID;

            // Convert to SNNS dataset
            s_log.DebugFormat("Exporting dataset to SNNS format");
            var snnsDataset = new SNNSDataSet();
            var snnsTrainFile = Path.GetTempFileName();
            foreach (var sp in patterns)
            {
                var input = new double[numInputs];
                var output = new double[numClasses];

                foreach (var feature in sp.Features)
                {
                    input[feature.Key - 1] = feature.Value;
                }

                output[sp.Label - m_minClassID] = 1;

                var pattern = new SNNSPattern(input, output);
                snnsDataset.Add(pattern);
            }

            // Create & train the MLP
            s_log.DebugFormat("Start learning");
            m_model = new MLP(numInputs, new[] { m_numOfHiddenNeurons }, numClasses) { SnnsDataSet = snnsDataset };
            m_model.Learn(m_epochs);
            File.Delete(snnsTrainFile);
        }

        public override void Forget()
        {
            m_model = null;
        }

        public override IDictionary<int, float> Predict(SparsePattern sparsePattern)
        {
            var input = new double[m_model.NumOfInput];

            foreach(var feature in sparsePattern.Features)
            {
                input[feature.Key - 1] = feature.Value;
            }

            var output = m_model.Recognize(input);

            var prediction = new Dictionary<int, float>();
            for (int i = 0; i < output.Length; i++)
            {
                prediction.Add(i+m_minClassID, (float)output[i]);
            }

            return prediction;
        }

        public override void Save(string sModelFilePath)
        {
            if(m_model != null)
            {
                m_model.Save(sModelFilePath);
                using(var sw = new StreamWriter(sModelFilePath+".mlp"))
                {
                    sw.WriteLine(m_minClassID);
                }
            }
        }

        public override void Load(string sModelFilePath)
        {
            m_model = MLP.loadNetModel(sModelFilePath);
            using (var sw = new StreamReader(sModelFilePath + ".mlp"))
            {
                m_minClassID = int.Parse(sw.ReadLine());
            }
        }
    }
}