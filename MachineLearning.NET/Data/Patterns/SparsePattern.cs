//
// $Id: SparsePattern.cs 21954 2010-11-03 08:27:35Z xpuser $
// 

using System.Collections.Generic;
using System.Linq;

namespace MachineLearning.NET.Data.Patterns
{
    /// <summary>
    /// A Sparse pattern is a pattern where a high
    /// number of features is zero, and therefore a
    /// more compact representation can be used for data.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2008/07/15</date>
    public class SparsePattern : IGenericPattern 
    {
        public SparsePattern(IDictionary<int, float> pattern, int nClass)
        {
            Features = pattern;
            Label = nClass;
        }

        public IDictionary<int, float> Features { get; private set; }

        public IDictionary<int,object> Explain {
            get;
            set;}

        /// <value>
        /// The class this pattern belongs to. Can be -1 if
        /// none is defined.
        /// </value>
        public int Label { get; private set; }

        /// <summary>
        /// Checks if this pattern has non-zero features.
        /// Otherwise this is a candidate non-valid pattern.
        /// </summary>
        /// <returns></returns>
        public bool HasNonZeroFeatures()
        {
            if(Features == null ) return false;
            return (from f in Features.Values where f != 0.0f select f).Any();
        }
    }
}