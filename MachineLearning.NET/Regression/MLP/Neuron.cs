/*
* Created on Apr 5, 2004
*
* Interfaccia per un generico neurone
*/
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MachineLearning.NET.Regression.MLP
{
    /// <author>  Ignazio
    /// 
    /// </author>
    /// <remarks>
    /// Integrata dal progetto ArteNeuralLib
    /// </remarks>
    public class Neuron
    {
        virtual public double Output
        {
            get
            {
                return output;
            }
			
        }
        virtual public double Delta
        {
            get
            {
                return delta;
            }
			
        }
        virtual public List<Synapse> OutLinks
        {
            get
            {
                return outlinks;
            }
			
        }
        virtual public List<Synapse> InLinks
        {
            get
            {
                return inlinks;
            }
			
        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <returns>
        /// </returns>
        /// <param name="b">
        /// </param>
        virtual public bool Valid
        {
            get
            {
                return _valid;
            }
			
            set
            {
                _valid = value;
            }
			
        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary>Un neurone puo' esistere ma </summary>
        /// <returns> Returns the deleted.
        /// </returns>
        /// <param name="deleted">The deleted to set.
        /// </param>
        virtual public bool Deleted
        {
            get
            {
                return deleted;
            }
			
            set
            {
                this.deleted = value;
            }
			
        }
        protected internal List<Synapse> inlinks;

        protected internal List<Synapse> outlinks;
		
        protected internal System.String label;
		
        protected internal double delta;
		
        protected internal double output; // range from 0.0 to 1.0
		
        protected internal double _aggregation;
		
        protected internal bool _valid = true;
		
        protected internal bool deleted = false;
		
        protected internal double sse_error = 0;
		
        protected internal bool stop_learn = false;
		
        public static double momentum = 0.9;
		
        public static double learningRate = 0.05;
		
        public Neuron(System.String l)
        {
            label = l;
            inlinks = new List<Synapse>();
            outlinks = new List<Synapse>();
            init();
        }

        public virtual void  init()
        {
            foreach(Synapse s in inlinks)
                s.Weight = Synapse.random.NextDouble() / 5.0;

            output = 0.0;
            delta = 0.0;
            _aggregation = 0.0;
        }
		
        public virtual void  removeInSynapse(Synapse s)
        {
            // remove Returns: true if the Vector contained the specified element
            if (!inlinks.Remove(s))
                throw new System.Exception("The neuron " + label + " doesn't contains the specified input synapse!");
        }
		
        public virtual void  removeOutSynapse(Synapse s)
        {
            // remove Returns: true if the Vector contained the specified element
            if (!outlinks.Remove(s))
                throw new System.Exception("The neuron " + label + " doesn't contains the specified output synapse!");
        }
		
        /// <summary>for an output neuron. </summary>
        public virtual void  computeBackpropDelta(double d)
        {
            if (!Valid || stop_learn || Deleted)
            {
                return ;
            }
            delta = (d - output) * output * (1.0 - output);
        }
		
        /// <summary>for a hidden neuron. </summary>
        public virtual void  computeBackpropDelta()
        {
            if (!Valid || stop_learn || Deleted)
            {
                return ;
            }
            double errorSum = 0.0;
            foreach (Synapse s in outlinks)
                errorSum += s.BackPropDelta;
            
            delta = output * (1.0 - output) * errorSum;
        }

        public void computeOutput()
        {
            _aggregation = 0.0;
            foreach (Synapse s in inlinks)
                _aggregation += s.from.Output * s.Weight;

            output = 1.0 / (1.0 + System.Math.Exp(-_aggregation)); // sigmoid function
        }

        public void computeWeight()
        {
            if (!Valid || stop_learn || Deleted)
                return;
            //System.Collections.IEnumerator e = inlinks.GetEnumerator();
            foreach (Synapse s in inlinks)
            {
                s.DeltaWeight = learningRate * delta * s.from.Output + momentum * s.DeltaWeight;
                s.Weight = s.Weight + s.DeltaWeight;
            }
        }

        public void print()
        {
            System.Console.Out.Write(label + "=" + output + ": ");
            foreach (Synapse synapse in outlinks)
                System.Console.Out.Write(synapse.to.label + "(" + synapse.Weight + ") ");

            System.Console.Out.WriteLine();
        }

        public override System.String ToString()
        {
            System.String str = "Input Weight of neuron " + label + ": ";
            for (int i = 0; i < (inlinks.Count - 1); i++)
                str += ("(" + inlinks[i].Weight + ") ");

            // bias
            str += ("(bias=" + inlinks[(inlinks.Count - 1)].Weight + ") ");
            str += ("(sse=" + sse_error + ") ");

            return str;
        }

        /// <param name="fp">
        /// </param>
        public virtual void save(System.IO.StreamWriter fp)
        {
            fp.Write("neuron " + label + "\n");
            fp.Write(string.Format(CultureInfo.InvariantCulture, "deleted {0} \n", deleted));
            foreach (Synapse s in inlinks)
                s.save(fp);
            fp.Write("\n");
        }

        /// <summary>Legge solo i dati relativi al neurone. </summary>
        /// <param name="fp">
        /// </param>
        public virtual void  open(System.IO.StreamReader fp)
        {
            char[] delimiter = " ".ToCharArray();
            string[] elements = StringUtils.EliminaStrVuote(fp.ReadLine().Split(delimiter));

            if (elements[0].ToLower().Equals("neuron"))
            {
                label = elements[1];
				
                // leggo il parametro deleted
                elements = StringUtils.EliminaStrVuote(fp.ReadLine().Split(delimiter));
                if (elements[0].ToLower().Equals("deleted"))
                    deleted = Boolean.Parse(elements[1]);
				
                // leggo i pesi
                elements = StringUtils.EliminaStrVuote(fp.ReadLine().Split(delimiter));
                int i = 0;
                foreach (Synapse s in inlinks)
                    s.open(Double.Parse(elements[i++], CultureInfo.InvariantCulture));
            }
            else
                throw new System.IO.IOException("Etichetta attesa 'neuron', trovata " + elements[0]);
        }
		
    }
}