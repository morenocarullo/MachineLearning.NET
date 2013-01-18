using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using log4net;
using MachineLearning.NET.Mapack;

namespace MachineLearning.NET.Optimization
{
    /// <summary>
    /// An iterative optimizater based on the Widrow-Hoff algorithm.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    internal class WidrowHoffOptimizer : ILMSOptimizer
    {
        private static readonly ILog s_log = LogManager.GetLogger(typeof(WidrowHoffOptimizer));
        private static readonly NumberFormatInfo s_numberFormat = CultureInfo.InvariantCulture.NumberFormat;

        private readonly int m_numOfEpochs;
        private readonly double m_learningRate;

        public WidrowHoffOptimizer(IDictionary<string,string> config)
        {
            m_numOfEpochs = config.ContainsKey("epochs") ? int.Parse(config["epochs"]) : 100;
            m_learningRate = config.ContainsKey("alpha") ? float.Parse(config["alpha"], s_numberFormat) : 0.05;
        }

        public Matrix Optimize(Matrix x, Matrix y)
        {
            var numOfOutputs = y.Columns;
            var numOfRbfs = x.Columns;
            var numOfPoints = y.Rows;
            var w = new Matrix(numOfRbfs, numOfOutputs);

            for (var epochId = 0; epochId < m_numOfEpochs; epochId++ )
            {
                // Update weights
                for (var rbfId = 0; rbfId < numOfRbfs; rbfId++)
                {
                    for (var outputId = 0; outputId < numOfOutputs; outputId++)
                    {
                        var thisOutputId = outputId;

                        for (var pointId = 0; pointId < numOfPoints; pointId++)
                        {
                            var thisPointId = pointId;
                            var difference = (from r in Enumerable.Range(0, numOfRbfs)
                                              select x[thisPointId, r] * w[r, thisOutputId]).Sum() - y[pointId,thisOutputId];

                            w[rbfId, outputId] = w[rbfId, outputId] - m_learningRate*(difference) *x[thisPointId, rbfId];
                        }
                    }
                }

                // Compute SSE
                if (epochId % 10 == 0)
                {
                    var sse = (from pointId in Enumerable.Range(0, numOfPoints)
                               from outputId in Enumerable.Range(0, numOfOutputs)
                               from rbfId in Enumerable.Range(0, numOfRbfs)
                               select Math.Pow(x[pointId, rbfId]*w[rbfId, outputId] - y[pointId, outputId], 2)).Sum();

                    s_log.DebugFormat("Epochs {0} SSE = {1}", epochId, sse);
                }
            }

            return w;
        }
    }
}