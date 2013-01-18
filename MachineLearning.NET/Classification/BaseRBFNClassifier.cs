//
// $Id: BaseRBFNClassifier.cs 21954 2010-11-03 08:27:35Z xpuser $
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using log4net;
using MachineLearning.NET.Data.Distances;
using MachineLearning.NET.Data.Patterns;
using MachineLearning.NET.Mapack;
using MachineLearning.NET.Optimization;

namespace MachineLearning.NET.Classification
{
    /// <summary>
    /// A generic implementation of an RBFN classifier
    /// </summary>
    /// <typeparam name="P"></typeparam>
    /// <creator>Moreno Carullo</creator>
    public class BaseRBFNClassifier<P> : BaseClassifier<P>
        where P : class, IGenericPattern
    {
        private static readonly ILog s_log = LogManager.GetLogger(typeof (BaseRBFNClassifier<P>));

        private static readonly NumberFormatInfo s_numberFormat = CultureInfo.InvariantCulture.NumberFormat;

        /// <summary>
        /// The user-requested value of M, the number of radial basis processing units
        /// </summary>
        protected readonly int m_numberOfBasisFunctions;

        /// <summary>
        /// The RBFN centroids (radial basis processing units)
        /// </summary>
        private P[] m_centroids;

        /// <summary>
        /// An array containing the sigma of each radial basis processing unit
        /// </summary>
        private double[] m_sigmas;

        /// <summary>
        /// The learned second level linear weights
        /// </summary>
        protected double[][] m_linearWeights;

        /// <summary>
        /// This dictionary maps problem-classes to array-ids
        /// </summary>
        protected readonly IDictionary<int,int> m_classesMap = new Dictionary<int, int>();

        private double[,] m_centroidsDistanceMatrix;

        private readonly IDistance<P> m_distance;

        private readonly ILMSOptimizer m_optimizer;
        protected readonly string m_sigmaConfig;
        private bool m_useBias;

        public override bool IsTrained
        {
            get
            {
                return
                    m_centroids != null && m_sigmas != null && m_linearWeights != null;
            }
        }

        protected BaseRBFNClassifier(IDictionary<string, string> configuration, IDistance<P> distance)
            : this(configuration)
        {
            m_distance = distance;
        }

        protected BaseRBFNClassifier(IDictionary<string,string> configuration)
        {
            m_sigmaConfig = configuration.ContainsKey("sigma") ? configuration["sigma"] : string.Empty;
            m_useBias = configuration.ContainsKey("bias") ? bool.Parse(configuration["bias"]) : true;
            m_numberOfBasisFunctions = int.Parse(configuration["M"]);
            m_optimizer = LMSOptimizerFactory.GetOptimizer(configuration.ContainsKey("optimizer") ? configuration["optimizer"] : "pi");

            s_log.InfoFormat("bias = {0}", m_useBias);
            s_log.InfoFormat("M = {0}", m_numberOfBasisFunctions);
        }

        public override void Train(IEnumerable<P> patterns)
        {
            var allPoints = patterns.ToList();
            var distinctLabels = (from p in allPoints orderby p.Label select p.Label).Distinct().ToArray();
            var classId = 0;
            foreach(var distinctLabel in distinctLabels)
            {
                m_classesMap.Add(distinctLabel, classId++);
            }

            s_log.InfoFormat("Minimum required memory: {0} Mb", (allPoints.Count*m_numberOfBasisFunctions*sizeof(double))/(1024*1024));

            // 1. select the centroids
            m_centroids = ChooseCentroids(allPoints, m_numberOfBasisFunctions, distinctLabels);

            // 2. compute centroid distance matrix & sigmas
            HashSet<int> toDelete;
            m_centroidsDistanceMatrix = ComputeCentroidsDistanceMatrix(m_centroids, m_distance, out toDelete);
            if (toDelete.Count > 0)
            {
                m_centroids = RemoveDuplicateCentroids(toDelete, m_centroids);
                m_centroidsDistanceMatrix = ComputeCentroidsDistanceMatrix(m_centroids, m_distance, out toDelete);
                if (toDelete.Count > 0)
                {
                    throw new Exception("Unable to remove duplicate centroids.");
                }
            }
            m_sigmas = ChooseSigmas(m_centroids, m_sigmaConfig, m_centroidsDistanceMatrix, m_numberOfBasisFunctions);

            // 3. learn linear weights
            m_linearWeights = LearnLinearWeights(allPoints);
        }

        protected T[] RemoveDuplicateCentroids<T>(HashSet<int> toDelete, T[] centroids)
        {
            if (toDelete.Count > 0)
            {
                s_log.InfoFormat("Removing {0} duplicate centroids.", toDelete.Count);
                var newCentroids = (from centroidId in Enumerable.Range(0, centroids.Length)
                                    where !toDelete.Contains(centroidId)
                                    select centroids[centroidId]).ToArray();
                return newCentroids;
            }
            return centroids;
        }

        protected double[][] LearnLinearWeights(IList<P> allPoints)
        {
            // The PHI matrix
            s_log.InfoFormat("Computing PHI");
            var phi = ComputeRBFNPhi(allPoints);

            // The target matrix T
            s_log.InfoFormat("Computing T");
            var targetsMatrix = new Matrix(allPoints.Count, m_classesMap.Count);
            for (var p = 0; p < targetsMatrix.Rows; p++)
            {
                targetsMatrix[p, m_classesMap[allPoints[p].Label]] = 1.0;
            }

            // Learn W such that I*W = T
            s_log.InfoFormat("Computing Pseudoinverse");
            var linearWeights = m_optimizer.Optimize(phi, targetsMatrix);
            s_log.DebugFormat("RBF LW: {0}", linearWeights);
            return linearWeights.Array;
        }

        public override void Forget()
        {
            m_linearWeights = null;
            m_centroids = null;
            m_classesMap.Clear();
        }

        public override IDictionary<int, float> Predict(P sparsePattern)
        {
            var output = Matrix.Multiply(ComputeRBFNPhi(new[] {sparsePattern}.ToList()),new Matrix(m_linearWeights));

            var results = (from c in m_classesMap
                           select new {Class = c.Key, Possibility = output[0, c.Value]}).ToDictionary(i => i.Class, i => (float) i.Possibility);

            return results;
        }

        public override void Save(string sModelFilePath)
        {
            //private P[] m_centroids;
            //private double[] m_sigmas;
            //private double[,] m_linearWeights;
        }

        public override void Load(string sModelFilePath)
        {
            //private P[] m_centroids;
            //private double[] m_sigmas;
            //private double[,] m_linearWeights;
        }

        protected virtual P[] ChooseCentroids(IEnumerable<P> allPoints, int numberOfCentroids, int[] distinctLabels)
        {
            // STEP1: get

            return (from p in allPoints orderby Guid.NewGuid() select p).Take(numberOfCentroids).ToArray();
        }

        protected virtual Matrix ComputeRBFNPhi(IList<P> allPoints)
        {
            Matrix phi;
            if (m_useBias)
            {
                phi = new Matrix(allPoints.Count, m_centroids.Length + 1);
                for (var a = 0; a < phi.Rows; a++)
                {
                    phi[a, m_centroids.Length] = 1;
                }
            }
            else
            {
                phi = new Matrix(allPoints.Count, m_centroids.Length);
            }

            for (var a = 0; a < phi.Rows; a++)
            {
                var x_p = allPoints[a];
                for (var c = 0; c < m_centroids.Length; c++)
                {
                    var x_c = m_centroids[c];
                    var distance = m_distance.Distance(x_p, x_c);
                    phi[a, c] = m_sigmas[c] > 0 ? Math.Exp(-distance / m_sigmas[c]) : 0;
                }
            }

            return phi;
        }

        protected double[,] ComputeCentroidsDistanceMatrix<T>(T[] centroids, IDistance<T> distanceMetric, out HashSet<int> toDelete)
        {
            var centroidsDistanceMatrix = new double[centroids.Length, centroids.Length];
            toDelete = new HashSet<int>();
            for (var centroid1 = 0; centroid1 < centroids.Length; centroid1++)
            {
                for (var centroid2 = 0; centroid2 < centroids.Length; centroid2++)
                {
                    if (centroid1 == centroid2) continue;
                    if (toDelete.Contains(centroid1)) continue;

                    var p1 = centroids[centroid1];
                    var p2 = centroids[centroid2];
                    var distance = distanceMetric.Distance(p1, p2);
                    centroidsDistanceMatrix[centroid1, centroid2] = distance;

                    if (distance == 0)
                    {
                        toDelete.Add(centroid2);
                    }
                }
            }
            return centroidsDistanceMatrix;
        }

        protected double[] ChooseSigmas<T>(ICollection<T> centroids, string sigmaConfiguration, double[,] centroidsDistanceMatrix, int numberOfBasisFunctions)
        {
            // If we have the parameter, use it
            double sigmaValue;
            if (!string.IsNullOrEmpty(sigmaConfiguration) && double.TryParse(sigmaConfiguration, NumberStyles.Float, s_numberFormat, out sigmaValue))
            {
                s_log.InfoFormat("Fixed sigma: {0}", sigmaConfiguration);
                return (from s in Enumerable.Range(0, numberOfBasisFunctions) select sigmaValue).ToArray();
            }

            // compute distances
            s_log.InfoFormat("Adaptive sigma, P-Mean {0}", sigmaConfiguration);

            int pMean = int.Parse(sigmaConfiguration.Substring(1));
            var sigmas = new double[centroids.Count];
            for (var centroidId = 0; centroidId < centroids.Count; centroidId++)
            {
                var thisCentroidId = centroidId;
                sigmas[centroidId] = (from otherCentroidId in Enumerable.Range(0, centroids.Count)
                                      where otherCentroidId != thisCentroidId
                                      orderby centroidsDistanceMatrix[thisCentroidId, otherCentroidId]
                                      select centroidsDistanceMatrix[thisCentroidId, otherCentroidId]).Take(pMean).Average() * 2;

                s_log.DebugFormat("rbf_{0} sigma = {1}", centroidId, sigmas[centroidId]);
            }

            return sigmas;
        }
    }
}