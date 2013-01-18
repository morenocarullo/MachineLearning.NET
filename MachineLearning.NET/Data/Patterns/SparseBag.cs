//
// $Id$
//

using System.Collections.Generic;
using System.Linq;

namespace MachineLearning.NET.Data.Patterns
{
    /// <summary>
    /// Represents a bag of instances for Multi-Instance Learning algorithm.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2009/12/22</date>
    public class SparseBag : IGenericPattern
    {
        public int Label
        {
            get; private set;
        }

        public string BagName { get; private set; }

        public IDictionary<int, float>[] Istances { get; private set; }

        public bool HasValidInstances
        {
            get{ return (from i in Istances
                         from f in i
                         where f.Value != 0.0 select f).Any();}
        }

        public SparseBag(int classId, string bagName, IDictionary<int, float>[] istances)
        {
            Label = classId;
            BagName = bagName;
            Istances = istances;
        }

        public override string ToString()
        {
            return string.Format("{0} with {1} istances", BagName, Istances.Length);
        }

        public static bool operator ==(SparseBag sp1, SparseBag sp2)
        {
            return ReferenceEquals(sp1, null) ? ReferenceEquals(sp2,null) : sp1.Equals(sp2);
        }

        public static bool operator !=(SparseBag sp1, SparseBag sp2)
        {
            return !(sp1 == sp2);
        }

        public override bool Equals(object obj)
        {
            if(ReferenceEquals(this,obj)) return true;
            if(ReferenceEquals(obj, null)) return false;
            var objAs = obj as SparseBag;
            if(objAs == null) return false;

            return
                objAs.Label == Label &&
                AreIstancesEqual(objAs.Istances, Istances);
        }

        public override int GetHashCode()
        {
            return (Label*397) ^ Istances.GetHashCode();
        }

        private static bool AreIstancesEqual(IDictionary<int,float>[] istances1, IDictionary<int,float>[] istances2)
        {
            if(istances1.Length != istances2.Length) return false;
            for(var istanceId=0; istanceId<istances1.Length; istanceId++)
            {
                var ist1ist = istances1[istanceId];
                var ist2ist = istances2[istanceId];

                if (ist1ist.Count != ist2ist.Count)
                {
                    return false;
                }

                foreach (var keyValue in ist1ist)
                {
                    if (!ist2ist.ContainsKey(keyValue.Key) || keyValue.Value != ist2ist[keyValue.Key])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public SparseBag RemoveEmptyIstances()
        {
            var istances = (from i in Istances
                            let instance = (from f in i
                                            where f.Value != 0.0
                                            select f).ToDictionary(f => f.Key, f => f.Value)
                            where instance.Any()
                            select instance).ToArray();

            return new SparseBag(Label, BagName, istances);
        }
    }
}