using System;
using System.Globalization;
using System.IO;
using System.Linq;
using log4net;
using MachineLearning.NET.Mapack;

namespace MachineLearning.NET.Optimization
{
    public static class OctaveUtil
    {
        private static readonly ILog s_log = LogManager.GetLogger(typeof (OctaveUtil));

        private static readonly NumberFormatInfo s_numberFormat = CultureInfo.InvariantCulture.NumberFormat;

        public static Matrix LoadOctaveMatrix(string tmpFilePath, string matrixName)
        {
            s_log.InfoFormat("Loading GNU Octave matrix from {0}", tmpFilePath);

            Matrix m = null;
            using (var sr = new StreamReader(tmpFilePath))
            {
                string line;
                var rows = 0;
                var columns = 0;
                var currentRow = 0;
                var name = String.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    // Header parsing
                    if (line.StartsWith("#"))
                    {
                        if (line.StartsWith("# rows: "))
                        {
                            rows = Int32.Parse(line.Substring("# rows: ".Length));
                        }
                        if (line.StartsWith("# columns: "))
                        {
                            columns = Int32.Parse(line.Substring("# columns: ".Length));
                        }
                        if(line.StartsWith("# name: "))
                        {
                            name = line.Substring("# name: ".Length);
                        }
                        continue;
                    }

                    // Matrix setup & check
                    if (name != matrixName)
                    {
                        throw new Exception("There was an error in Octave when saving the pseudoinverse.");
                    }
                    if(rows > 0 && columns > 0 && m == null)
                    {
                        m = new Matrix(rows, columns);
                    }

                    // Row reading
                    var parts = (from c in line.Split(new[]{" "},StringSplitOptions.RemoveEmptyEntries) select Single.Parse(c, s_numberFormat)).ToArray();
                    for(var colId=0; colId<parts.Length; colId++)
                    {
                        m[currentRow, colId] = parts[colId];
                    }
                    currentRow++;
                }
            }

            return m;
        }
    }
}