using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Data.Distances
{
    [Distance(Name="euclidean")]
    internal class SparsePatternEuclidean : IDistance<SparsePattern>
    {
        public double Distance(SparsePattern sp1, SparsePattern sp2)
        {
            return Euclidean.Distance(sp1.Features, sp2.Features);
        }
    }
}