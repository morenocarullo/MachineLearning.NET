using System;
using System.Globalization;
using System.IO;
using System.Linq;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Data.IO
{
    /// <summary>
    /// This dataset writer permits to save a collection of patterns for analysis in GNU Octave.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    public class OctaveDatasetWriter : IDisposable
    {
        private readonly StreamWriter m_streamWriter;
        private int m_patternId;
        private int m_maxFeatureId;
        private readonly IFormatProvider m_formatProvider = CultureInfo.InvariantCulture.NumberFormat;

        protected bool HeaderWritten
        {
            get;
            set;
        }

        public OctaveDatasetWriter(string fullFileName)
        {
            var fileName = Path.GetFileNameWithoutExtension(fullFileName);
            var replaceChars = new[] { '.', '-' };
            foreach (var c in replaceChars)
            {
                fileName = fileName.Replace(c.ToString(), "");
            }

            m_streamWriter = new StreamWriter(
                Path.Combine(
                    Path.GetDirectoryName(fullFileName),
                    fileName + Path.GetExtension(fullFileName)));
        }

        public void Dispose()
        {
            m_streamWriter.Dispose();
        }

        public void Write(SparsePattern sparsePattern)
        {
            if(sparsePattern.Explain == null) return;

            if (!HeaderWritten)
            {
                foreach (var explanation in sparsePattern.Explain)
                {
                    m_streamWriter.WriteLine("classes({0}).name = '{1}';", explanation.Key, explanation.Value);
                }
                HeaderWritten = true;
            }

            var features = new float[m_maxFeatureId + 1];
            features[0] = sparsePattern.Label;
            for (var featId = 0; featId < m_maxFeatureId; featId++)
            {
                features[featId + 1] = sparsePattern.Features[featId + 1];
            }

            var featuresAsString = (from f in features select f.ToString(m_formatProvider)).ToArray();

            m_streamWriter.WriteLine("pattern({0},:) = [{1}];", ++m_patternId, string.Join(",", featuresAsString));
        }
    }
}