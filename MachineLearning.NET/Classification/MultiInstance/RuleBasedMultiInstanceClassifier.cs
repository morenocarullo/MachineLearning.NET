using System;
using System.Collections.Generic;
using System.Linq;
using MachineLearning.NET.Data.Patterns;
using MachineLearning.NET.RuleSystem;

namespace MachineLearning.NET.Classification.MultiInstance
{
    [Classifier(Name="rules")]
    internal class RuleBasedMultiInstanceClassifier : BaseClassifier<SparseBag>
    {
        private readonly RuleSet _positiveClassRules;

        public override bool IsTrained
        {
            get { return true; }
        }

        public RuleBasedMultiInstanceClassifier(IDictionary<string, string> configuration)
        {
            if (!configuration.ContainsKey("class+1"))
            {
                throw new ArgumentException("You must provide a rule set for 'class+1'.");
            }

            _positiveClassRules = RuleSetFactory.BuildRule(configuration["class+1"]);
        }

        public override void Forget()
        {
        }

        public override IDictionary<int, float> Predict(SparseBag sparseBag)
        {
            foreach(var istance in sparseBag.Istances)
            {
                var properties = (from f in istance select new {FeatureID = f.Key, FeatureValue = f.Value}).ToDictionary(i => (object) i.FeatureID.ToString(), i => (object) i.FeatureValue);
                if (_positiveClassRules.VerifiedBy(properties))
                {
                    return new Dictionary<int, float>{{1,1.0f}};
                }
            }
            return new Dictionary<int, float>{{-1,1.0f}};
        }

        public override void Save(string sModelFilePath)
        {
        }

        public override void Load(string sModelFilePath)
        {
        }

        public override void Train(IEnumerable<SparseBag> patterns)
        {
        }
    }
}