using System;

namespace MachineLearning.NET.Data.IO
{
    public interface IDatasetWriter<P> : IDisposable
    {
        /// <summary>
        /// Write a new pattern to disk specifying if to reduce the sparseness or not.
        /// </summary>
        /// <param name="spPattern">the pattern to write</param>
        void Write(P spPattern);
    }
}