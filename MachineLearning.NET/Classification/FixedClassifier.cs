using System;
using System.Collections.Generic;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Classification
{
    /// <summary>
    /// A fixed classifier always responds with probability 1.0 for a given class.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2009/07/08</date>
    [Classifier(Name="fixed")]
    public class FixedClassifier<P> : BaseClassifier<P>
        where P :class, IGenericPattern
    {
        public FixedClassifier(IDictionary<string,string> config)
        {
            #region Pre-Execution checks

            if (!config.ContainsKey("class"))
            {
                throw new ArgumentException("You must provide the 'class' entry","config");
            }

            #endregion

            ClassToPredict = int.Parse(config["class"]);
        }

        private int ClassToPredict { get; set; }

        public override bool IsTrained
        {
            get { return true; }
        }

        public override void Forget()
        {
        }

        public override IDictionary<int, float> Predict(P sparsePattern)
        {
            return new Dictionary<int, float> { { ClassToPredict, 1.0f } };
        }

        public override int PredictWinner(P spPattern)
        {
            return ClassToPredict;
        }

        public override void Save(string sModelFilePath)
        {
        }

        public override void Load(string sModelFilePath)
        {
        }

        public override void Train(IEnumerable<P> patterns)
        {
        }
    }
}