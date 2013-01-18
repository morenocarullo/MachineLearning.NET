using System.Collections;
using System.Collections.Generic;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Data.IO
{
    /// <summary>
    /// Provides an extension applied to all IDatasetReader
    /// </summary>
    internal static class DatasetReaderExtensions
    {
        public static IEnumerable<P> ToEnumerable<P>(this IDatasetReader<P> sr)
            where P:class, IGenericPattern
        {
            return new EnumerableDatasetReader<P>(sr);
        }
    }

    internal class EnumerableDatasetReader<P> : IEnumerable<P>, IEnumerator<P>
        where P :class, IGenericPattern
    {
        private readonly IDatasetReader<P> m_datasetReader;

        public P Current { get; private set; }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public EnumerableDatasetReader(IDatasetReader<P> streamReader)
        {
            m_datasetReader = streamReader;
        }

        public IEnumerator<P> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            Reset();
        }

        public bool MoveNext()
        {
            Current = m_datasetReader.Next();
            return (Current != null);
        }

        public void Reset()
        {
            m_datasetReader.Reset();
        }
    }
}