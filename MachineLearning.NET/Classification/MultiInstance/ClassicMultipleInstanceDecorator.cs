using System.Collections.Generic;
using System.Linq;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Classification.MultiInstance
{
    /// <summary>
    /// This classifier permits to use a classic ML classifier as a Multiple-Instance classifier.
    /// Of course this approach is naive; custom MI models should be used in a MI learning setting.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2010/02/15</date>
    [Classifier(Name="mi")]
    internal class ClassicMultipleInstanceDecorator : BaseClassifier<SparseBag>
    {
        private readonly IClassifier<SparsePattern> m_classicClassifier;

        public ClassicMultipleInstanceDecorator(IDictionary<string, string> configuration)
        {
            var modelAndConfig = string.Format("{0}:{1}", configuration["model"],
                                               string.Join(",", (from c in configuration
                                                                 select string.Format("{0}={1}", c.Key, c.Value)).ToArray()));

            m_classicClassifier = ClassifierFactory.GetClassicClassifier(modelAndConfig);
        }

        public override bool IsTrained
        {
            get { return m_classicClassifier.IsTrained; }
        }

        /// <summary>
        /// Train by converting the Bags into Patterns.
        /// </summary>
        /// <param name="bags">the bags to train from</param>
        public override void Train(IEnumerable<SparseBag> bags)
        {
            var patterns = from bag in bags
                           from p in bag.Istances
                           select new SparsePattern(p, bag.Label);

            m_classicClassifier.Train(patterns);
        }

        public override void Forget()
        {
            m_classicClassifier.Forget();
        }

        public override int PredictWinner(SparseBag spPattern)
        {
            var predictions = from p in spPattern.Istances
                              let sp = new SparsePattern(p, spPattern.Label)
                              select m_classicClassifier.PredictWinner(sp);

            // The bag is positive if there is at least one positive example
            return predictions.Where(p => p == +1).Any() ? 1 : -1;
        }

        public override IDictionary<int, float> Predict(SparseBag sparseBag)
        {
            var winner = PredictWinner(sparseBag);

            return new Dictionary<int, float> { { winner, 1.0f } };
        }

        public override void Save(string sModelFilePath)
        {
            m_classicClassifier.Save(sModelFilePath);
        }

        public override void Load(string sModelFilePath)
        {
            m_classicClassifier.Load(sModelFilePath);
        }
    }
}