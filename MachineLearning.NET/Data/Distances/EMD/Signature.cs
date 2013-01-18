using System;
using System.Collections.Generic;

namespace MachineLearning.NET.Data.Distances.EMD
{
    public class Signature
    {
        //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        // private LinkedList < Features > features;
        private List<Features> features;
        private int n;

        //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        //private LinkedList < Double > weights;
        private List<Double> weights;
		
        public Signature(double[] weights) : this()
        {
            N = weights.Length;
            insertWeights(weights);
        }

        [Obsolete]
        public Signature()
        {
            //UPGRADE_TODO: Class 'java.util.LinkedList' was converted to 'System.Collections.ArrayList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilLinkedList'"
            features = new List<Features>();
            //UPGRADE_TODO: Class 'java.util.LinkedList' was converted to 'System.Collections.ArrayList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilLinkedList'"
            weights = new List<Double>();
        }

        virtual public int N
        {
            get
            {
                return n;
            }
			
            set
            {
                this.n = value;
            }
        }

        public virtual void  insertWeights(double[] w)
        {
            for (var i = 0; i < w.Length; i++)
            {
                this.weights.Add(w[i]);
            }
        }
		
		
        public virtual double getWeight(int i)
        {
            return this.weights[i];
        }
    }
}