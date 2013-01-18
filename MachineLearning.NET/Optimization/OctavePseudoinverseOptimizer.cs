using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using log4net;
using MachineLearning.NET.Mapack;

namespace MachineLearning.NET.Optimization
{
    internal class OctavePseudoinverseOptimizer : ILMSOptimizer
    {
        private static readonly ILog s_log = LogManager.GetLogger(typeof(OctavePseudoinverseOptimizer));

        private static readonly NumberFormatInfo s_numberFormat = CultureInfo.InvariantCulture.NumberFormat;

        private readonly string m_octavePath = ConfigurationManager.AppSettings["octave.bin"];

        public OctavePseudoinverseOptimizer(IDictionary<string, string> config)
        {            
        }

        public Matrix Optimize(Matrix x, Matrix t)
        {
            s_log.InfoFormat("Pseudoinverse of M_{0}x{1}", x.Rows, x.Columns);

            var tmpFilePath = Path.GetTempFileName();
            using (var sw = new StreamWriter(tmpFilePath))
            {
                sw.WriteLine("# Created by ArteML");
                sw.WriteLine("# name: m");
                sw.WriteLine("# type: matrix");
                sw.WriteLine("# rows: {0}", x.Rows);
                sw.WriteLine("# columns: {0}", x.Columns);
                for (var rowId = 0; rowId < x.Rows; rowId++)
                {
                    for (var colId = 0; colId < x.Columns; colId++)
                    {
                        sw.Write(" {0}", x[rowId, colId].ToString(s_numberFormat));
                    }
                    sw.Write(Environment.NewLine);
                }
            }

            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.Arguments = "-qf";
            process.StartInfo.FileName = m_octavePath;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.StandardInput.WriteLine("load {0};", tmpFilePath);
            process.StandardInput.WriteLine("p=pinv(m);");
            process.StandardInput.WriteLine("save {0} p;", tmpFilePath);
            process.StandardInput.WriteLine("quit;");
            process.WaitForExit();

            var m = OctaveUtil.LoadOctaveMatrix(tmpFilePath, "p");

            if (File.Exists(tmpFilePath))
            {
                File.Delete(tmpFilePath);
            }

            return Matrix.Multiply(m,t);
        }
    }
}