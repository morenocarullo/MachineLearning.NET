using System.Collections.Generic;
using MachineLearning.NET.Data.Patterns;
using MachineLearning.NET.Evaluation;

namespace MachineLearning.NET.Classification
{
    /// <summary>
    /// A base implementation for classifiers.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2008/07/15</date>
    public abstract class BaseClassifier<P> : IClassifier<P>
        where P : class, IGenericPattern
    {
        public abstract bool IsTrained
        {
            get;
        }

        public virtual bool HasProbabilities
        {
            get { return false; }
        }

        public abstract void Forget();

        public abstract IDictionary<int, float> Predict(P sparsePattern);

        public abstract void Save(string sModelFilePath);

        public abstract void Load(string sModelFilePath);

        public abstract void Train(IEnumerable<P> patterns);

        public void Test(IEnumerable<P> patterns, ComposedQualityEvaluator<int> qualityEvaluator)
        {
            foreach(var pattern in patterns)
            {
                var nWinnerClass = PredictWinner(pattern);
                qualityEvaluator.AddData(pattern.Label, nWinnerClass);
            }
        }

        public virtual int PredictWinner(P spPattern)
        {
            var predictions = Predict(spPattern);

            var nWinnerClass = -1;
            var fWinnerActivation = float.MinValue;

            foreach (var nClass in predictions.Keys)
            {
                if (predictions[nClass] > fWinnerActivation)
                {
                    fWinnerActivation = predictions[nClass];
                    nWinnerClass = nClass;
                }
            }

            return nWinnerClass;
        }
    }
}