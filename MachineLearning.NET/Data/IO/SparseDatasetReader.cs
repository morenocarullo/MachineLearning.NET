using System;
using System.Collections;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Data.IO
{
    /// <summary>
    /// This class permits to treat the sparse dataset
    /// as a 'stream' of patterns. Models should only
    /// call the 'Next()' method until it returns null.
    /// </summary>
    internal class SparseDatasetReader : IDatasetReader<SparsePattern>
    {
        private static readonly NumberFormatInfo s_numberFormat = CultureInfo.InvariantCulture.NumberFormat;
        private readonly StreamReader m_srDataset;

        public SparseDatasetReader(string sSparseDatasetPath)
        {
            m_srDataset = File.OpenText(sSparseDatasetPath);
        }
		
        public SparseDatasetReader(Stream sparseDatasetStream)
        {
            m_srDataset = new StreamReader(sparseDatasetStream);
        }

        public SparseDatasetReader(StreamReader streamReader)
        {
            m_srDataset = streamReader;
        }

        #region IDisposable Members

        public void Dispose ()
        {
            m_srDataset.Close();
        }

        #endregion

        /// <summary>
        /// Get the next pattern of the dataset and returns it,
        /// if the end has been reached null is returned instead.
        /// </summary>
        public SparsePattern Next()
        {
            IDictionary<int,float> features = new Dictionary<int,float>();
            int id;
            char[] splittingCharsSpace = {' '};
            char[] splittingCharsColumn = {':'};
			
            var sLine = m_srDataset.ReadLine();
			
            if( string.IsNullOrEmpty(sLine) )
            {
                return null;
            }
			
            var parts = sLine.Split(splittingCharsSpace, StringSplitOptions.RemoveEmptyEntries);

            // Class
            int nClass;
            if(!int.TryParse(parts[0], out nClass))
            {
                throw new FormatException("Unable to parse: "+parts[0]);
            }
			
            // Features
            foreach(var part in parts)
            {
                var subparts = part.Split(splittingCharsColumn, StringSplitOptions.RemoveEmptyEntries);
                if( subparts.Length != 2 ) continue;
				
                features.Add( int.Parse(subparts[0], s_numberFormat), float.Parse(subparts[1], s_numberFormat) );
            }
			
            return new SparsePattern(features, nClass);
        }

        public void Reset()
        {
            m_srDataset.BaseStream.Seek(0, SeekOrigin.Begin);
            m_srDataset.DiscardBufferedData();
        }

        public IEnumerator<SparsePattern> GetEnumerator()
        {
            return new SparseDatasetReader(new StreamReader(m_srDataset.BaseStream)).ToEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}