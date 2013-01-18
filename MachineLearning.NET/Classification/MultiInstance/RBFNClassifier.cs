using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MachineLearning.NET.Data.Distances;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Classification.MultiInstance
{
    [Classifier(Name = "rbfn")]
    internal class RBFNClassifier : BaseRBFNClassifier<SparseBag>
    {
        private static readonly ILog s_log = LogManager.GetLogger(typeof (RBFNClassifier));

        public RBFNClassifier(IDictionary<string, string> configuration)
            : base(configuration, DistanceFactory.GetDistanceMetric<SparseBag>(configuration["distance"]))
        {
        }

        protected override SparseBag[] ChooseCentroids(IEnumerable<SparseBag> allPoints, int numberOfCentroids, int[] distinctLabels)
        {
            var centroids = (from l in distinctLabels
                             let c = (from p in allPoints where p.Label == l orderby Guid.NewGuid() select p).Take(numberOfCentroids/distinctLabels.Length)
                             from p in c
                             orderby Guid.NewGuid()
                             select p).Distinct().ToArray();

            // TODO: merge duplicate centroids?
            //       apply clustering algorithm?

            return centroids.ToArray();
        }
    }
}