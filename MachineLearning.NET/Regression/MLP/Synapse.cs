using System.Globalization;

namespace MachineLearning.NET.Regression.MLP
{
    /// <summary>Una rete � formata da un insieme di layer ed ogni 
    /// Layer � formato da un insieme di Neuron.
    /// I Neuron di due livelli consecutivi sono linkati
    /// tra di loro da Synapse
    /// 
    /// </summary>
    /// <author>  Ignazio
    /// 
    /// </author>
    public class Synapse
    {
        virtual public double Weight
        {
            get
            {
                return weight;
            }
			
            set
            {
                weight = value;
            }
			
        }
        virtual public double DeltaWeight
        {
            get
            {
                return w_delta;
            }
			
            set
            {
                w_delta = value;
            }
			
        }
        virtual public double BackPropDelta
        {
            get
            {
                double error = 0.0;
                error = to.delta * Weight;
                return error;
            }
			
        }
        private double weight;
        private double w_delta;
        protected internal double w_accum_delta;
        internal Neuron from;
        internal Neuron to;
        public static System.Random random = new System.Random();
		
        internal Synapse(Neuron f, Neuron t)
        {
            from = f;
            to = t;
            weight = random.NextDouble() / 5.0;
            w_delta = 0.0;
            w_accum_delta = 0;
            f.outlinks.Add(this);
            t.inlinks.Add(this);
        }
		
        /// <param name="fp">
        /// </param>
        public virtual void  save(System.IO.StreamWriter fp)
        {
            fp.Write(string.Format(CultureInfo.InvariantCulture, " {0:0.#####}", weight));
        }
		
        /// per comodit� passo solo il valore
        /// <param name="fp">
        /// </param>
        public virtual void open(double w)
        {
            weight = w;
        }
    }
}