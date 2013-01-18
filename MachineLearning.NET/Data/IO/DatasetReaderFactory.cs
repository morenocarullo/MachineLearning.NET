using System;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Data.IO
{
    public static class DatasetReaderFactory
    {
        public static IDatasetReader<P> GetDatasetReader<P>(string datasetFilePath)
        {
            if(typeof(P) == typeof(SparsePattern))
            {
                return new SparseDatasetReader(datasetFilePath) as IDatasetReader<P>;
            }
            if(typeof(P) == typeof(SparseBag))
            {
                return new SparseBagReader(datasetFilePath) as IDatasetReader<P>;
            }
            throw new ArgumentException("Unknown generic type");
        }
    }
}