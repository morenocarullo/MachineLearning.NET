using System.Collections.Generic;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Classification.Classic
{
    [Classifier(Name = "nn")]
    public class ClassicNearestNeighbor : NearestNeighbor<SparsePattern>
    {
        public ClassicNearestNeighbor(IDictionary<string, string> configuration) : base(configuration) { }
    }
}