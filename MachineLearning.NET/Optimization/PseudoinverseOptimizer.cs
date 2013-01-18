using System.Collections.Generic;
using log4net;
using MachineLearning.NET.Mapack;

namespace MachineLearning.NET.Optimization
{
    internal class PseudoinverseOptimizer : ILMSOptimizer
    {
        private static readonly ILog s_log = LogManager.GetLogger(typeof (PseudoinverseOptimizer));

        public PseudoinverseOptimizer(IDictionary<string,string> config)
        {            
        }

        public Matrix Optimize(Matrix x, Matrix t)
        {
            s_log.InfoFormat("Pseudoinverse of M_{0}x{1}", x.Rows, x.Columns);

            var m = x.Inverse;

            return Matrix.Multiply(m, t);
        }
    }
}