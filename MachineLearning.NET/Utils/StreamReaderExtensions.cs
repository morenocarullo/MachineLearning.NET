using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MachineLearning.NET.Utils
{
    /// <summary>
    /// This simple extender permits to get an enumeration out of a StreamReader.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    public static class StreamReaderExtensions
    {
        public static IEnumerable<string> ToEnumerable(this StreamReader sr)
        {
            return new EnumerableStreamReader(sr);
        }
    }

    internal class EnumerableStreamReader : IEnumerable<string>, IEnumerator<string>
    {
        private readonly StreamReader m_streamReader;
        private string m_current;

        public EnumerableStreamReader(StreamReader streamReader)
        {
            m_streamReader = streamReader;
        }

        public IEnumerator<string> GetEnumerator()
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
            m_current = m_streamReader.ReadLine();
            return (m_current != null);
        }

        public void Reset()
        {
            m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            m_streamReader.DiscardBufferedData();
        }

        public string Current
        {
            get { return m_current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}