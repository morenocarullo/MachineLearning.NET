using System;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Data.IO
{
    public static class DatasetWriterFactory
    {
        public static IDatasetWriter<P> GetDatasetWriter<P>(string datasetFilePath)
        {
            if (typeof(P) == typeof(SparsePattern))
            {
                return new SparseDatasetWriter(datasetFilePath) as IDatasetWriter<P>;
            }
            if (typeof(P) == typeof(SparseBag))
            {
                return new SparseBagWriter(datasetFilePath) as IDatasetWriter<P>;
            }
            throw new ArgumentException("Unknown generic type");
        }
    }
}