//
// $Id: FeatureNormalization.cs 21954 2010-11-03 08:27:35Z xpuser $
//  

using System.Collections.Generic;

namespace MachineLearning.NET.Data
{
    /// <summary>
    /// Utility class to normalize features.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2009/03/19</date>
    public static class FeatureNormalization
    {
        /// <summary>
        /// Perform cosine normalization.
        /// </summary>
        /// <param name="features">the features to normalize</param>
        /// <returns>the normalized features</returns>
        public static IDictionary<int,float> Cosine(IDictionary<int,float> features)
        {
            var length = 0.0f;
			
            foreach(var featVal in features.Values)
            {
                length += featVal;
            }

            var normalizedFeatures = new Dictionary<int, float>();
            foreach(var feature in features)
            {
                normalizedFeatures.Add(feature.Key, feature.Value/length);
            }
			
            return normalizedFeatures;
        }
    }
}