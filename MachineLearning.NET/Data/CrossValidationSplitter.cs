//
// $Id: CrossValidationSplitter.cs 21954 2010-11-03 08:27:35Z xpuser $
//

using System.Collections.Generic;
using System.Linq;

namespace MachineLearning.NET.Data
{
    /// <summary>
    /// This class permits to perform a CrossValidation split of a given foldness
    /// on a generic collection of data.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <typeparam name="P">the type of considered data</typeparam>
    public class CrossValidationSplitter<P>
    {
        private int Foldness { get; set; }

        public CrossValidationSplitter(int foldness)
        {
            Foldness = foldness;
        }

        public void GetSplits(IEnumerable<P> data, out IEnumerable<P> trs, out IEnumerable<P> tes, int part)
        {
            var trsList = new List<P>();
            var tesList = new List<P>();
            trs = trsList;
            tes = tesList;

            var blockSize = data.Count()/Foldness;

            int position = 0;

            var startBlock = (part-1)*blockSize;
            var endBlock = startBlock + blockSize - 1;

            foreach (var pattern in data)
            {
                if (position >= startBlock && position <= endBlock)
                {
                    tesList.Add(pattern);
                }
                else
                {
                    trsList.Add(pattern);
                }

                position++;
            }
        }
    }
}