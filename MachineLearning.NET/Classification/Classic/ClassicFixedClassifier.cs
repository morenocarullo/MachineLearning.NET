using System.Collections.Generic;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Classification.Classic
{
    [Classifier(Name="fixed")]
    public class ClassicFixedClassifier : FixedClassifier<SparsePattern>
    {
        public ClassicFixedClassifier(IDictionary<string, string> configuration) : base(configuration) { }
    }
}