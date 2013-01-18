//
// $Id$
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using MachineLearning.NET.Data.Patterns;
using MachineLearning.NET.Utils;

namespace MachineLearning.NET.Data.IO
{
    /// <summary>
    /// Writes a collection of bags in the MILL format.
    /// http://www.cs.cmu.edu/~juny/MILL/
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    // <instance_name>,<bag_name>,<label>,<feature>:<value> ... <feature>:<value>
    public class SparseBagReader : IDatasetReader<SparseBag>, IEnumerable<SparseBag>
    {
        private static readonly NumberFormatInfo s_numberFormat = CultureInfo.InvariantCulture.NumberFormat;

        private readonly IList<SparseBag> m_bagList;

        private int m_currentBag = -1;

        private class BagElements
        {
            public int Label;
            public readonly List<IDictionary<int, float>> Istances = new List<IDictionary<int, float>>();
        }

        public SparseBagReader(string datasetFilePath)
        {
            m_bagList = LoadAll(datasetFilePath);
        }

        /// <summary>
        /// Load all sparse bags.
        /// An example line of the file is:
        /// 188_1+1,MUSK-188,1,1:42,2:-198,3:-109,4:-75,5:-117,6:11,7:23,8:-88,9:-28,10:-27,11:-232,12:-212,13:-66,14:-286,15:-287,16:-300,17:-57,18:-75,19:-192,20:-184,21:-66,22:-18,23:-50,24:111,25:110,26:18,27:-18,28:-127,29:25,30:63,31:-117,32:-114,33:-47,34:9,35:-135,36:26,37:-175,38:73,39:-143,40:71,41:-177,42:-85,43:-30,44:-282,45:-280,46:-249,47:-135,48:-11,49:-139,50:-105,51:-142,52:-32,53:-9,54:-48,55:147,56:1,57:40,58:-170,59:35,60:33,61:-101,62:-195,63:26,64:-5,65:-144,66:48,67:-165,68:18,69:-133,70:15,71:-146,72:-148,73:-146,74:-246,75:-216,76:-181,77:-37,78:-212,79:-216,80:-174,81:-20,82:8,83:-120,84:-38,85:-7,86:11,87:-156,88:-39,89:-7,90:82,91:-202,92:-15,93:-115,94:-46,95:26,96:-49,97:-166,98:32,99:-141,100:76,101:-206,102:26,103:-257,104:-289,105:-304,106:-163,107:-117,108:-17,109:-247,110:-283,111:-244,112:-64,113:-35,114:-32,115:-10,116:57,117:110,118:25,119:6,120:-117,121:80,122:149,123:130,124:-110,125:-134,126:-14,127:35,128:51,129:11,130:-187,131:13,132:-138,133:-67,134:-163,135:-201,136:-19,137:45,138:-115,139:-11,140:-37,141:-100,142:77,143:78,144:60,145:-178,146:-102,147:-118,148:-33,149:-104,150:41,151:-77,152:-120,153:-111,154:-168,155:-54,156:-195,157:-238,158:-74,159:-129,160:-120,161:-38,162:30,163:48,164:-37,165:6,166:30
        /// </summary>
        /// <param name="datasetFilePath"></param>
        /// <returns></returns>
        private static IList<SparseBag> LoadAll(string datasetFilePath)
        {
            using(var sr = new StreamReader(datasetFilePath))
            {
                var bags = new Dictionary<string, BagElements>();
                var lines = sr.ToEnumerable();
                foreach(var line in lines)
                {
                    var parts = line.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);

                    // Bag-specific parts
                    var bagName = parts[1];
                    var bagLabel = int.Parse(parts[2]);

                    // Conver features to a dictionary
                    var istanceFeatures = (from f in parts.Skip(3) let fparts = f.Split(':')
                                           select new
                                                      {
                                                          FeatureID=int.Parse(fparts[0]),
                                                          Value = float.Parse(fparts[1], s_numberFormat)
                                                      }).ToDictionary(i=>i.FeatureID, i=>i.Value);
                    
                    if(!bags.ContainsKey(bagName))
                    {
                        bags.Add(bagName, new BagElements{Label = bagLabel});
                    }
                    bags[bagName].Istances.Add(istanceFeatures);
                }

                return (from b in bags select new SparseBag(b.Value.Label, b.Key, b.Value.Istances.ToArray())).ToList();
            }
        }

        public SparseBag Next()
        {
            m_currentBag++;
            if(m_currentBag < m_bagList.Count)
            {
                return m_bagList[m_currentBag];
            }
            return null;
        }

        public void Reset()
        {
            m_currentBag = -1;
        }

        public void Dispose()
        {
        }

        public IEnumerator<SparseBag> GetEnumerator()
        {
            return this.ToEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}