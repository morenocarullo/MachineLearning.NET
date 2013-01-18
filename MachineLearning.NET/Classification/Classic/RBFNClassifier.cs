using System.Collections.Generic;
using MachineLearning.NET.Data.Distances;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Classification.Classic
{
    [Classifier(Name="rbfn")]
    class RBFNClassifier : BaseRBFNClassifier<SparsePattern>
    {
        public RBFNClassifier(IDictionary<string, string> configuration)
            : base(configuration, DistanceFactory.GetDistanceMetric<SparsePattern>(configuration["distance"]))
        {
        }
    }
}
