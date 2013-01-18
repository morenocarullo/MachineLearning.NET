//  
// $Id: SparseDatasetWriter.cs 21954 2010-11-03 08:27:35Z xpuser $
//  

using System;
using System.IO;
using System.Globalization;
using System.Linq;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Data.IO
{
    /// <summary>
    /// An utility class that permits to write a collection of sparse patterns
    /// to a dataset file.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2008/09/25</date>
    internal class SparseDatasetWriter : IDatasetWriter<SparsePattern>
    {
        private static readonly NumberFormatInfo s_numberFormat = CultureInfo.InvariantCulture.NumberFormat;
        private readonly StreamWriter m_swDataset;

        public SparseDatasetWriter(string sDatasetFilePath)
        {
            m_swDataset = File.CreateText(sDatasetFilePath);
        }

        #region IDisposable Members

        public void Dispose ()
        {
            m_swDataset.Flush();
            m_swDataset.Dispose();
        }

        #endregion
		
        /// <summary>
        /// Write a new pattern to disk specifying if to reduce the sparseness or not.
        /// </summary>
        /// <param name="spPattern">the pattern to write</param>
        public void Write(SparsePattern spPattern)
        {
            if(spPattern.Features.Count == 0)
            {
                throw new ArgumentException("Can't write invalid pattern with no features!");
            }

            // NOTE: the format CANNOT be modified since it is a standard format used by ML tools.
            m_swDataset.Write("{0} ", spPattern.Label);

            foreach(var nFeature in from k in spPattern.Features.Keys orderby k select k)
            {
                if (Single.IsNaN(spPattern.Features[nFeature]))
                {
                    throw new Exception(string.Format("Invalid value NaN in feature {0}", nFeature));
                }

                if (spPattern.Features[nFeature] != 0)
                {
                    m_swDataset.Write(string.Format(s_numberFormat, "{0}:{1} ", nFeature, spPattern.Features[nFeature]));
                }
            }

            m_swDataset.WriteLine();
        }
    }
}