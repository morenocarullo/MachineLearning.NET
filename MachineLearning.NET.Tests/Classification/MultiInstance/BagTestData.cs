using System.Collections.Generic;
using System.Linq;
using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Tests.Classification.MultiInstance
{
    internal class BagTestData
    {
        private readonly int _name;
        private readonly int _label;
        private readonly double[][] _points;

        public int Name
        {
            get { return _name; }
        }

        public int Label
        {
            get { return _label; }
        }

        public double[][] Points
        {
            get { return _points; }
        }

        public BagTestData(int label, int name, double[][] points)
        {
            _name = name;
            _label = label;
            _points = points;
        }

        static internal IEnumerable<SparseBag> ToSparseBags(BagTestData[] bags)
        {
            var sparseBags = new SparseBag[bags.Length];
            for (var bagId = 0; bagId < sparseBags.Length; bagId++)
            {
                var istances = new IDictionary<int, float>[bags[bagId].Points.Length];
                for (var pointId = 0; pointId < bags[bagId].Points.Length; pointId++)
                {
                    var j = 1;
                    var features = (from f in bags[bagId].Points[pointId] select f).ToDictionary(i => (j++), i => (float)i);
                    istances[pointId] = features;
                }

                sparseBags[bagId] = new SparseBag(bags[bagId].Label, bags[bagId].Name.ToString(), istances);
            }
            return sparseBags;
        }
    }
}
