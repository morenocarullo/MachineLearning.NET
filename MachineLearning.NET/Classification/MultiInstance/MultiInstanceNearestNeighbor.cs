using System.Collections.Generic;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Classification.MultiInstance
{
    [Classifier(Name = "nn")]
    public class MultiInstanceNearestNeighbor : NearestNeighbor<SparseBag>
    {
        public MultiInstanceNearestNeighbor(IDictionary<string, string> configuration) : base(configuration) { }
    }
}