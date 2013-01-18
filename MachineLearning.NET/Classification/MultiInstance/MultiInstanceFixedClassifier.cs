using System.Collections.Generic;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Classification.MultiInstance
{
    [Classifier(Name="fixed")]
    public class MultiInstanceFixedClassifier : FixedClassifier<SparseBag>
    {
        public MultiInstanceFixedClassifier(IDictionary<string, string> configuration) : base(configuration) { }
    }
}