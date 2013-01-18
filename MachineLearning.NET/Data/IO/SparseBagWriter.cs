//
// $Id$
//

using System.Globalization;
using System.IO;
using System.Linq;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Data.IO
{
    /// <summary>
    /// Writes a collection of bags in the MILL format.
    /// http://www.cs.cmu.edu/~juny/MILL/
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    // <instance_name>,<bag_name>,<label>,<feature>:<value> ... <feature>:<value>
    public class SparseBagWriter : IDatasetWriter<SparseBag>
    {
        private static readonly NumberFormatInfo s_numberFormat = CultureInfo.InvariantCulture.NumberFormat;
        private readonly StreamWriter m_streamWriter;

        public SparseBagWriter(string datasetFilePath)
        {
            m_streamWriter = File.CreateText(datasetFilePath);
        }

        public void Write(SparseBag bag)
        {
            for(var instanceId=0; instanceId<bag.Istances.Length; instanceId++)
            {
                var featureString = string.Join(",", (from f in bag.Istances[instanceId] select string.Format("{0}:{1}", f.Key, f.Value.ToString(s_numberFormat))).ToArray());

                m_streamWriter.WriteLine("{0},{1},{2},{3}", instanceId, bag.BagName, bag.Label, featureString);
            }
        }

        public void Dispose()
        {
            m_streamWriter.Flush();
            m_streamWriter.Dispose();
        }
    }
}