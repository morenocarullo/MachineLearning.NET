using System;
using System.Collections.Generic;
using System.Globalization;
using log4net;
using MachineLearning.NET.Data.IO;

namespace MachineLearning.NET.Regression.MLP
{
    /// <summary>Una Rete e' formata da un insieme di layer ed ogni 
    /// Layer e' formato da un insieme di Neuron.
    /// I Neuron di due livelli consecutivi sono linkati
    /// tra di loro da Synapse
    /// </summary>
    /// <remarks>
    /// Integrata dal progetto ArteNeuralLib
    /// </remarks>
    /// <author>Ignazio Gallo</author>
    public sealed class MLP 
    {
        private static readonly ILog s_log = LogManager.GetLogger(typeof (MLP));

        /// <summary>
        /// Get the output values of the MLP
        /// </summary>
        public double[] Output
        {
            get
            {
                double[] oS = new double[outputLayer.Count];
                for (int i = 0; i < outputLayer.Count; i++)
                    oS[i] = outputLayer[i].Output;

                return oS;
            }
			
        }

        public double CurrentSSError
        {
            get {return error;}
        }

        /// <summary>
        /// Returns the layers number.
        /// </summary>
        public int NumLayers
        {
            get{return layers.Count;}
        }

        /// <summary>
        /// Sets the dataset for training;
        /// </summary>
        public SNNSDataSet SnnsDataSet
        {
            set
            {
                if (value != null)
                {
                    if (_snnsDataSet != value)
                    {
                        if (!isTrainCompatible(value))
                        {
                            _snnsDataSet = null;
                            throw new DataSetNotSpecifiedException("***  tentativo di caricamento dataset non compatibile con la rete!");
                        }
                        _snnsDataSet = value;
                        s_log.InfoFormat("Dataset caricato!");
                    }
                }
                else
                {
                    throw new DataSetNotSpecifiedException("*** Error: tentativo di caricamento dataset NULL!");
                }
            }
			
        }

        public long NumEpoche
        {
            get{return num_epoche;}
        }

        public String ModelType
        {
            get{return MODEL_NAME;}
        }

        public int NumOfOutput
        {
            get
            {
                return outputLayer.NumNeurons;
            }
        }

        public int NumOfInput
        {
            get
            {
                return inputLayer.NumNeurons - 1;
            }
        }

        public double BrakeEpoche
        {
            get
            {
                return this.brakeEpoche;
            }
			
            set
            {
                this.brakeEpoche = value;
            }
			
        }

        internal List<Layer> layers;
        private SNNSDataSet _snnsDataSet = null;
        private Layer inputLayer;
        private Layer outputLayer;
        private double error;
        private long num_epoche = 0;
        private double brakeEpoche = 100;
		
        /// <summary><code>MODEL_NAME</code> useful to determine the network type in a model file. </summary>
        public const System.String MODEL_NAME = "MLP";

        private MLP() { }
		
        /// <summary>Crea una rete con un numero variabile di hidden layer.
        /// Connette tutti i Layers: full connected feedforward
        /// 
        /// </summary>
        /// <param name="num_input_neurons">numero di neuroni nell'input layer
        /// </param>
        /// <param name="num_neurons_per_hidden">array di int rappresentante 
        /// il numero di neuroni nell'hidden layer
        /// </param>
        /// <param name="num_output_neurons">numero di neuroni nell'output layer
        /// </param>
        public MLP(int num_input_neurons, int[] num_neurons_per_hidden, int num_output_neurons)
        {
            this.initMlp(num_input_neurons, num_neurons_per_hidden, num_output_neurons);
        }

        public double[] Recognize(double[] iS)
        {
            if(!isInPatternCompatible(iS))
            {
                throw new DataSetNotSpecifiedException("***  tentativo di caricamento di un input pattern non compatibile con la rete!");
            }
            initInputs(iS);
            propagate();
            return Output;
        }
		
        public void  Learn(int iterations)
        {
            if (_snnsDataSet == null)
                throw new DataSetNotSpecifiedException("*** Null training set!");
            s_log.InfoFormat("Start training phase!");
            for (int i = 0; i < iterations; i++)
            {
                // accumulate total error over each epoch
                error = 0.0;
                foreach (var pat in _snnsDataSet)
                {
                    learnPattern(pat.Input, pat.Output);
                    error += computeError(pat.Output);
                }
                num_epoche++;
                s_log.InfoFormat("Epoch: " + num_epoche + " error: " + error);
            }
        }
		
        internal void  learnPattern(double[] iS, double[] oS)
        {
            initInputs(iS);
            propagate();
            bpAdjustWeights(oS);
        }
		
        internal void  initInputs(double[] iS)
        {
            int i = 0;
            for (i = 0; i < iS.Length; i++)
            {
                inputLayer[i].output = iS[i];
            }
            inputLayer[i].output = 1.0; // TODO: � proprio il bias? verificare  
        }
		
        internal void  propagate()
        {
            for (int i = 1; i < layers.Count; i++)
            {
                layers[i].computeOutputs();
            }
        }
		
        internal double computeError(double[] oS)
        {
            double sum = 0.0;
            double tmp = 0;

            for (int i = 0; i < outputLayer.Count; i++ )
            {
                tmp = oS[i] - outputLayer[i].Output;
                sum += tmp * tmp;
            }
            return sum / 2.0;
        }
		
        internal void  bpAdjustWeights(double[] oS)
        {
            outputLayer.computeBackpropDeltas(oS);
            for (int i = layers.Count - 2; i >= 1; i--)
                layers[i].computeBackpropDeltas();
            outputLayer.computeWeights();
            for (int i = layers.Count - 2; i >= 1; i--)
                layers[i].computeWeights();
        }
		
        public void  print()
        {
            System.Console.Out.WriteLine("*** MLP:");
            foreach (Layer layer in layers)
                layer.print();
        }
        public override System.String ToString()
        {
            System.String str = "*** MLP:";
            foreach (Layer layer in layers)
                str += layer;

            return str;
        }
        public System.String toHtmlString()
        {
            System.String str = "MLP:<br>";
            foreach (Layer layer in layers)
                str += " " + layer + "<br>";

            return str;
        }
		
        /// <summary>Connette con una Synapse due neuroni 
        /// appartenenti a due distinti layer
        /// Non viene effettuato alcun controllo sulla diversit� dei Layer
        /// 
        /// </summary>
        /// <param name="sourceLayer">
        /// </param>
        /// <param name="destLayer">
        /// </param>
        private void  connectTwoLayers(Layer sourceLayer, Layer destLayer)
        {
            int num_neuron_from = sourceLayer.NumNeurons;
            int num_neuron_to = destLayer.NumNeurons;
            foreach (Neuron fromNeuron in sourceLayer)
                foreach (Neuron toNeuron in destLayer)
                    new Synapse(fromNeuron, toNeuron);
        }
		
        public void  initMlp(LayerInfo[] layerInfo)
        {
            layers = new List<Layer>();
            int numi = layerInfo[0].numOfNeurons;
            inputLayer = new Layer("I", numi + 1, Layer.LAYER_INPUT); // plus the bias
            int numo = layerInfo[layerInfo.Length - 1].numOfNeurons;
            outputLayer = new Layer("O", numo, Layer.LAYER_OUTPUT);
            layers.Add(inputLayer);
            layers.Add(outputLayer);
            // creo gli Hidden Layer
            if (layerInfo.Length > 2)
            {
                for (int i = 1; i < (layerInfo.Length - 1); i++)
                {
                    int num_neuron = layerInfo[i].numOfNeurons;
                    addHiddenLayer(num_neuron, "H" + i);
                }
                this.connectAllLayers();
            }
            else
                try
                {
                    connectTwoLayers(inputLayer, outputLayer);
                }
                catch (System.Exception e)
                {
                    Console.Error.WriteLine(e.StackTrace);
                }
			
            error = 10.0;
        }
		
        /// <summary>
        /// Crea una rete MLP ed inizializza tutti i pesi con valori random.
        /// </summary>
        /// <param name="numInputs">Number of input neurons</param>
        /// <param name="num_neurons_per_hidden">Number of hidden neurons, for each hidden layer</param>
        /// <param name="numOutputs">Number of output neurons</param>
        private void  initMlp(int numInputs, int[] num_neurons_per_hidden, int numOutputs)
        {
            layers = new List<Layer>();
            inputLayer = new Layer("I", numInputs + 1, Layer.LAYER_INPUT); // plus the bias
            outputLayer = new Layer("O", numOutputs, Layer.LAYER_OUTPUT);
            layers.Add(inputLayer);
            layers.Add(outputLayer);
			
            // creo gli Hidden Layer
            if (num_neurons_per_hidden != null)
            {
                for (int i = 0; i < num_neurons_per_hidden.Length; i++)
                {
                    addHiddenLayer(num_neurons_per_hidden[i], "H" + i);
                }
            }
            this.connectAllLayers();
			
            error = 10.0;
        }
		
        /// <summary> Verifica se training e rete sono compatibili</summary>
        private bool isTrainCompatible(SNNSDataSet ds)
        {
            s_log.InfoFormat("MLP: In#{0} Out#{1} / DS: In#{2} Out#{3}", inputLayer.Count-1, outputLayer.Count, ds.NbInputUnit, ds.NbOutputUnit);

            if (ds.NbInputUnit != (inputLayer.Count - 1))
                return false;
            if (ds.NbOutputUnit != outputLayer.Count)
                return false;
            return true;
        }
        /// <summary> Verifica se pattern di input e rete sono compatibili</summary>
        private bool isInPatternCompatible(double[] inpat)
        {
            if (inpat.Length != (inputLayer.Count - 1))
                return false;
            return true;
        }
        /// <summary>Crea una Synapse tra il neurone aggiuntivo in input (+1)
        /// ed i neuroni del destLayer
        /// </summary>
        /// <param name="destLayer:">
        /// </param>
        private void  biasConnect(int destLayer)
        {
            Layer lcurr = getLayer(destLayer);
            int num_neuron = lcurr.NumNeurons;
            Neuron bias = inputLayer[inputLayer.NumNeurons - 1];
            for (int n = 0; n < num_neuron; n++)
                new Synapse(bias, lcurr[n]);
        }
        /// <summary>Aggiunge un Layer in coda alla lista di Layers, prima dell'output Layer
        /// 
        /// </summary>
        /// <param name="num_neuron:">numero di neuroni per il Layer da creare
        /// </param>
        /// <param name="name">: nome del Layer (I,Hn,O)
        /// </param>
        private void  addHiddenLayer(int num_neuron, System.String name)
        {
            layers.Insert(layers.Count - 1, new Layer(name, num_neuron, Layer.LAYER_HIDDEN));
        }
		
        /* Connette tutti i layers in modo feedforward.
		* aggiunge le connessioni al bias di input
		*/
        private void  connectAllLayers()
        {
            Layer l1, l2;
            l1 = layers[0]; //input layer
            for (int i = 1; i < layers.Count; i++)
            {
                l2 = layers[i];
                connectTwoLayers(l1, l2);
                l1 = l2;
            }
			
            // connect all neurons with bias
            // il primo livello hidden � stato gi� connesso
            for (int i = 2; i < layers.Count; i++)
                biasConnect(i);
        }
		
        /// <param name="index:">indice del Layer
        /// </param>
        /// <returns> il Layer index o null nel caso in cui questo non esista
        /// </returns>
        public Layer getLayer(int index)
        {
            if (index >= layers.Count || index < 0)
                return null;
            return layers[index];
        }
		
        /// <summary>
        /// Factory method: legge da file il modello e lo crea.
        /// </summary>
        /// <param name="model_file_name"></param>
        /// <returns>ritorna il modello creato.</returns>
        public static MLP loadNetModel(string model_file_name) 
        {
            System.IO.StreamReader fp = new System.IO.StreamReader(model_file_name);
            MLP _nn;

            char[] delimiter = " ".ToCharArray();
            string[] elements = fp.ReadLine().Split(delimiter);

            if (elements[0].ToLower().Equals("model_type")) {
                String model = elements[1];
                if (model.ToLower().Equals(MLP.MODEL_NAME.ToLower())) {
                    _nn = new MLP();
                } else
                    throw new System.IO.IOException("*** unknown model type.\n");
            } else
                throw new System.IO.IOException("*** unknown file format.\n");

            // leggo i parametri della rete
            _nn.open(fp);

            fp.Close();

            return _nn;
        }

        public void  open(System.IO.StreamReader fp)
        {
            char[] delimiter = " ".ToCharArray();
            string[] elements = StringUtils.EliminaStrVuote(fp.ReadLine().Split(delimiter));

            if (elements[0].ToLower().Equals("neurons_per_layer"))
            {
                List<int> neurs = new List<int>();
                int numi, numo;
                // num inputs
                numi = System.Int32.Parse(elements[1], CultureInfo.InvariantCulture);
                // num hiddens + outputs
                for (int i=2; i<elements.Length; i++)
                    neurs.Add(System.Int32.Parse(elements[i], CultureInfo.InvariantCulture));
                // num output
                numo = neurs[neurs.Count - 1];
                neurs.RemoveAt(neurs.Count - 1);

                int[] numH = neurs.ToArray();

                this.initMlp(numi, numH, numo);
            }
            else
                throw new System.IO.IOException("*** Formato del file non valido: manca la keyword 'neurons_per_layer'.\n");

            elements = StringUtils.EliminaStrVuote(fp.ReadLine().Split(delimiter));
            if (elements[0].ToLower().Equals("error"))
            {
                error = System.Double.Parse(elements[1], CultureInfo.InvariantCulture);
            }
            else
                throw new System.IO.IOException("*** Formato del file non valido: manca la keyword 'error'.\n");

            elements = StringUtils.EliminaStrVuote(fp.ReadLine().Split(delimiter));
            if (elements[0].ToLower().Equals("num_epoche"))
            {
                num_epoche = System.Int32.Parse(elements[1], CultureInfo.InvariantCulture);
            }
            else
                throw new System.IO.IOException("*** Formato del file non valido: manca la keyword 'num_epoche'.\n");
			
            for (int i=1; i< layers.Count; i++) // skip the input layer
                layers[i].open(fp);
        }
		
        public void  Save(System.String model_file_name)
        {
            var fp = new System.IO.StreamWriter(model_file_name);

            fp.Write("model_type " + ModelType + "\n");
            fp.Write("neurons_per_layer ");
            fp.Write(" " + (layers[0].NumNeurons - 1)); //input layer 
            for (int i = 1; i < layers.Count; i++) // skip the input layer
                fp.Write(" " + layers[i].NumNeurons); 
            fp.Write("\n");
			
            fp.Write(string.Format(CultureInfo.InvariantCulture, "error {0:0.#####} \n", error));
            fp.Write("num_epoche " + num_epoche + "\n");

            for (int i = 1; i < layers.Count; i++) // skip the input layer
                layers[i].save(fp);
			
            fp.Close();
        }
    }
}