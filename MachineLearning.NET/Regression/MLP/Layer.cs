using System.Collections.Generic;

namespace MachineLearning.NET.Regression.MLP
{
    /// <summary>Memorizza alcune informazioni di un Layer.</summary>
    /// <author>  ignazio
    /// </author>
    /// <remarks>
    /// Integrata dal progetto ArteNeuralLib
    /// </remarks>
    public class LayerInfo
    {
        internal int numOfNeurons;
        internal System.String typeOfLayer;

        public LayerInfo(int numNeuron, System.String type)
        {
            this.numOfNeurons = numNeuron;
            this.typeOfLayer = type;
        }
    }

    /// <summary> Una rete e' formata da un insieme di layer ed ogni Layer e' formato da un
    /// insieme di Neuron. I Neuron di due livelli consecutivi sono linkati tra di
    /// loro da Synapse</summary>
    /// <author>Ignazio Gallo</author>
    /// <remarks>
    /// Integrata dal progetto ArteNeuralLib
    /// </remarks>
    public class Layer : List<Neuron>
    {
        /// <summary> Ritorna il numero di neuroni del livello compreso il bias nel caso di
        /// input layer
        /// 
        /// </summary>
        /// <returns> size of neuron Vector
        /// </returns>
        virtual protected internal int NumNeurons
        {
            get {return this.Count;}
        }
        /// <summary>Ritorna il nome del livello</summary>
        /// <returns>  nome del livello
        /// </returns>
        virtual public System.String Label
        {
            get {return label;}
        }
        /// <summary>ritorna il tipo del neurone: INPUT, HIDDEN, OUTPUT</summary>
        virtual public int Type
        {
            get { return this.type;}
        }
		
        //protected internal List<Neuron> neurons;
		
        //protected int size;
        protected internal System.String label;
		
        public const int LAYER_INPUT = 1;
		
        public const int LAYER_HIDDEN = 2;
		
        public const int LAYER_OUTPUT = 3;
		
        private int type;
		
        public Layer(System.String layerLabel, int numOfNeurons, int layerType)
        {
            this.label = layerLabel;
            this.type = layerType;
            for (int i = 0; i < numOfNeurons; i++)
                Add(new Neuron(label + "_" + i));
        }
		
        public virtual void  init()
        {
            foreach (Neuron neuron in this)
                neuron.init();
        }
		
        public virtual void  computeOutputs()
        {
            foreach (Neuron neuron in this)
                neuron.computeOutput();
        }
		
        /// <summary>Compute Backprop Deltas for output neurons. </summary>
        public virtual void  computeBackpropDeltas(double[] out_Renamed)
        {
            int i = 0; //TODO verificare la corrispondebza tra indice i e neurone
            foreach (Neuron neuron in this)
                neuron.computeBackpropDelta(out_Renamed[i++]);
        }
		
        /// <summary>Compute Backprop Deltas for hidden neurons. </summary>
        public virtual void  computeBackpropDeltas()
        {
            foreach (Neuron neuron in this)
                neuron.computeBackpropDelta();
        }
		
        public virtual void  computeWeights()
        {
            foreach (Neuron neuron in this)
                neuron.computeWeight();
        }
		
        public virtual void  print()
        {
            foreach (Neuron neuron in this)
                neuron.print();
        }
		
        public override System.String ToString()
        {
            System.String str = label + "[";
            if (label.ToUpper().Equals("I".ToUpper()))
                str += ((this.NumNeurons - 1) + "+bias");
            else
                str += this.NumNeurons;
            return str + "]; ";
        }
		
        public virtual System.String toHtmlString()
        {
            System.String str = "";
            str += "<table cellpadding=\"2\" cellspacing=\"2\" border=\"1\"\n";
            str += " style=\"text-align: left; width: 100%;\">";
            str += "\t<tbody>";
            str += "\t<tr>";
            str += ("\t  <td style=\"vertical-align: top; width: 15%;\"><b>" + label + "</b><br>");
            str += "\t  </td>";
            str += "\t  <td style=\"vertical-align: top;\">Num neuroni: ";
            if (label.ToUpper().Equals("I"))
                str += ((this.NumNeurons - 1) + "+bias");
            else
                str += this.NumNeurons;
            str += "<br>";
            foreach (Neuron neuron in this)
            {
                if (!(label.ToUpper().Equals("I")))
                {
                    str += neuron + "<br>";
                }
            }
            str += "\t  </td>";
            str += "\t</tr>";
            str += "\t</tbody>";
            str += "</table>";
            return str;
        }
		
        //public virtual System.Drawing.Bitmap toImage(int width, int height)
        //{
        //    LayerImage limage = new LayerImage(width, height);
        //    System.Collections.IEnumerator n = neurons.GetEnumerator();
        //    //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
        //    while (n.MoveNext())
        //    {
        //        //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
        //        Neuron neuro = (Neuron) n.Current;
        //        if (neuro.Deleted || !neuro.Valid)
        //            continue;
        //        limage.addFunction(getWeigthPlot(neuro));
        //    }
        //    limage.plotFunctions();
        //    return (System.Drawing.Bitmap) limage;
        //}
		
        ///// <summary> vettore di vettori di pesi uscenti</summary>
        //protected internal virtual Function getWeigthPlot(Neuron neuron)
        //{
        //    System.Collections.ArrayList in_link = neuron.InLinks;
        //    int num_in = in_link.Count;
        //    Point2d[] points = new Point2d[num_in - 1];
        //    // per ogni link in input al neurone
        //    for (int i = 0; i < (num_in - 1); i++)
        //    {
        //        points[i] = new Point2d();
        //        points[i].x = i;
        //        points[i].y = ((Synapse) in_link[i]).Weight;
        //    }
        //    return new Function(neuron.label, points);
        //}
		
        /// <param name="fp"> DataOutputStream
        /// </param>
        public virtual void  save(System.IO.StreamWriter fp)
        {
            fp.Write("layer " + label + "\n");
            foreach (Neuron neuron in this)
                neuron.save(fp);
        }
		
        public virtual void  open(System.IO.StreamReader fp)
        {
            string[] elements = StringUtils.EliminaStrVuote(fp.ReadLine().Split(" ".ToCharArray()));
            if (elements[0].ToLower().Equals("layer"))
            {
                label = elements[1];
                foreach (Neuron neuron in this)
                    neuron.open(fp);
            }
        }


    }
}