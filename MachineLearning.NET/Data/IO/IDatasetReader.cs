//
// $Id$
//

using System;
using System.Collections.Generic;

namespace MachineLearning.NET.Data.IO
{
    public interface IDatasetReader<P> : IDisposable, IEnumerable<P>
    {
        /// <summary>
        /// Get the next pattern of the dataset and returns it,
        /// if the end has been reached null is returned instead.
        /// </summary>
        P Next();

        /// <summary>
        /// Return to the start of the dataset.
        /// </summary>
        void Reset();
    }
}