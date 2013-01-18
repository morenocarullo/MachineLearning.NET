using System;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using MachineLearning.NET.Data.Patterns;
using MachineLearning.NET.Regression.MLP;

namespace MachineLearning.NET.Data.IO
{
    /// <summary>
    /// Classe per la lettura, scrittura e modifica di un training set formato SNNS.
    /// </summary>
    /// <remarks>
    /// Integrata dal progetto ArteNeuralLib
    /// </remarks>
    /// <creator>Ignazio Gallo</creator>
    public class SNNSDataSet : List<SNNSPattern>
    {

        private const String TRAINING_SET = "SNNS pattern definition file V1.4";
        private const String GENERATED_AT = "generated at";
        private const String COMMENT = "#";
        private const String NO_PATTERNS = "No. of patterns";
        private const String NO_INPUTS = "No. of input units";
        private const String NO_OUTPUTS = "No. of output units";

        private char[] blanks = " \n\r\t".ToCharArray();

        private CultureInfo cultureInfo = CultureInfo.InvariantCulture;

        private int nbInputUnit = -1;
        /// <summary>
        /// Dimensione dell'input pattern.
        /// </summary>
        public int NbInputUnit
        {
            get { return nbInputUnit; }
        }

        private int nbOutputUnit = -1;
        /// <summary>
        /// Dimensione dell'output pattern.
        /// </summary>
        public int NbOutputUnit
        {
            get { return nbOutputUnit; }
        }
        private bool bModified = false;
        public bool IsModified
        {
            get { return bModified; }
            set { bModified = value; }
        }
        private string sFileName;
        public string FileName
        {
            get { return sFileName; }
        }


        /// <summary>
        /// Crea un nuovo DataSet non Configurato.
        /// La configurazione verra' fatta durante l'inserimento
        /// del primo pattern <seealso cref="addPattern()"></seealso>.
        /// </summary>
        public SNNSDataSet()
        {
        }
		
        /// <summary>
        /// Definisce un nuovo dataset con dimensione dei patterns
        /// di input e output fissati.
        /// </summary>
        /// <param name="numIn">Dimensione dell'input pattern.</param>
        /// <param name="numOut">Dimensione dell'output pattern.</param>
        public SNNSDataSet(int numIn, int numOut)
        {
            nbInputUnit = numIn;
            nbOutputUnit = numOut;
        }
        /// <summary>
        /// Costruttore.
        /// Legge un dataset da file e lo carica in RAM.
        /// </summary>
        /// <param name="fileName"></param>
        public SNNSDataSet(String fileName)
        {
            this.sFileName = Path.GetFullPath(fileName);

            if (File.Exists(this.sFileName))
                loadFromFile(this.sFileName);
            else
                throw new System.IO.FileNotFoundException("File " + fileName + " not found!");
        }


        /// <summary>
        /// Add a pattern to the training set.
        /// </summary>
        /// <param name="p"></param>
        new public void Add(SNNSPattern p)
        {
            VerifyPatternCompatibility(p);
            base.Add(p);
            this.IsModified = true;
        }

        /// <summary>
        /// Verifica la compatibiltà del pattern.
        /// Se e' il primo lo usa per configurare il DataSet.
        /// Se non e' compatibile con la conf del dataSet genera una
        /// Exception("Invalid Pattern size!")
        /// </summary>
        /// <param name="p">Pattern da validare</param>
        private void VerifyPatternCompatibility(SNNSPattern p)
        {
            // se e' il primo pattern
            // allora configuro le dimensioni di input e output.
            if (nbOutputUnit == -1)
                nbOutputUnit = p.Output.Length;
            if (nbInputUnit == -1)
                nbInputUnit = p.Input.Length;

            if (nbInputUnit != p.Input.Length || nbOutputUnit != p.Output.Length)
                throw new Exception("Invalid Pattern size!");
        }
		
        /// <summary>
        /// DA TESTARE:
        /// </summary>
        /// <param name="collection"></param>
        new public void AddRange(IEnumerable<SNNSPattern> collection)
        {
            foreach(SNNSPattern p in collection)
                VerifyPatternCompatibility(p);
			
            base.AddRange(collection);
            this.IsModified = true;
        }

        /// <summary>
        /// Salva il trainingSet su un file.
        /// </summary>
        /// <param name="name">Path assoluto del file su cui salvare.</param>
        public void SaveToFile(string name)
        {
            if (!bModified)
                return;

            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamWriter sr = new StreamWriter(name))
                {
                    writeHeader(sr);
                    // reads all the patterns
                    for (int p = 0; p < this.Count; p++)
                    {
                        writePattern(sr, this[p].Input, "Input pattern " + p + ":" + this[p].Comment );
                        writePattern(sr, this[p].Output, "Output pattern " + p + ":" + this[p].Comment);
                    }
                    sr.Flush();
                    //Console.WriteLine("PatternSet salvalto in " + name);
                    bModified = false;
                    sFileName = name;
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                throw new IOException("The training file '" + sFileName + "' could not be read:\n" +
                                      e.Message);
            }
        }

        public void SaveOctaveToFile(string fileName)
        {
            //if (!bModified)
            //	return;

            using (StreamWriter sr = new StreamWriter(fileName))
            {
                string[] datasetInputArray = new string[Count];
                string[] datasetOutputArray = new string[Count];
				
                for (int p = 0; p < this.Count; p++)
                {
                    double[] valuesInput = this[p].Input;
                    double[] valuesOutput = this[p].Output;
                    string[] valuesInputArray = new string[valuesInput.Length];
                    string[] valuesOutputArray = new string[valuesOutput.Length];

                    for (int i = 0; i < valuesInput.Length; i++)
                    {
                        valuesInputArray[i] = String.Format(cultureInfo, "{0:0.#####}", valuesInput[i]);
                    }
					
                    for (int i = 0; i < valuesOutput.Length; i++)
                    {
                        valuesOutputArray[i] = String.Format(cultureInfo, "{0:0.#####}", valuesOutput[i]);
                    }
					
                    datasetInputArray[p]  = string.Join(",", valuesInputArray);
                    datasetOutputArray[p] = string.Join(",", valuesOutputArray);
                }
				
                // Scrivi input stuff
                StringBuilder sb = new StringBuilder("MydatasetIn = [");
                sb.Append( string.Join(";", datasetInputArray) );
                sb.Append("];");
                sr.WriteLine(sb.ToString());
				
                // Scrivi input stuff
                StringBuilder sb2 = new StringBuilder("MydatasetOut = [");
                sb2.Append( string.Join(";", datasetOutputArray) );
                sb2.Append("];");
                sr.WriteLine(sb2.ToString());
				
                sr.Flush();
                bModified = false;
            }
        }

        private void loadFromFile(string fileName)
        {
            //m_Patterns = new List<Pattern>();
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(fileName))
                {
                    int nbPattern = readHeader(sr);
                    // reads all the patterns
                    for (int p = 0; p < nbPattern; p++)
                    {
                        double[] inps = readInputPattern(sr);
                        double[] outs = readOutputPattern(sr);
                        Add(new SNNSPattern(inps, outs));
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                throw new IOException("The training file '" + fileName + "' could not be read:\n" +
                                      e.Message);
            }
        }

        private double[] readOutputPattern(StreamReader sr)
        {
            double[] outs = new double[nbOutputUnit];
            int p = 0;
            while (p < nbOutputUnit)
            {
                String line = readNextSignificantLine(sr);
                String[] strArr = StringUtils.EliminaStrVuote(line.Split(blanks));
                for (int i = 0; i < strArr.Length; i++)
                {
                    outs[p++] = float.Parse(strArr[i], cultureInfo);
                }
            }
            return outs;
        }

        // legge sempre il punto come separatore decimale
        private double[] readInputPattern(StreamReader sr)
        {
            double[] inp = new double[nbInputUnit];
            int p = 0;
            while (p < nbInputUnit)
            {
                String line = readNextSignificantLine(sr);
                String[] strArr = StringUtils.EliminaStrVuote(line.Split(blanks));
                for (int i = 0; i < strArr.Length; i++)
                {
                    inp[p++] = float.Parse(strArr[i], cultureInfo);
                }
            }
            return inp;
        }

        // scrive sempre il punto come separatore decimale
        private void writePattern(StreamWriter sr, double[] values, string comment)
        {
            String outs = "#" + comment + "\n";
            for (int i = 0; i < values.Length; i++)
            {
                outs += String.Format(cultureInfo, "{0:0.#####} ", values[i]);
            }
            sr.Write(outs + "\n");
        }

        private void writeHeader(StreamWriter sr)
        {
            // scrivo l'intestazione
            sr.Write(
                TRAINING_SET + "\n" +
                GENERATED_AT + " " + DateTime.Now + "\n\n\n" +
                NO_PATTERNS + "     : " + this.Count + "\n" +
                NO_INPUTS + "  : " + nbInputUnit + "\n" +
                NO_OUTPUTS + " : " + nbOutputUnit + "\n\n"
                );
        }

        /// <summary>
        /// Legge l'intestazione di un file di training.
        /// </summary>
        /// <param name="sr"></param>
        /// <returns>Ritorna il numero di patterns contenuto nel file.</returns>
        private int readHeader(StreamReader sr)
        {
            String line = sr.ReadLine();
            int nbPatterns = 0;
            if (!line.StartsWith(TRAINING_SET))
                throw new IOException("This isn't a valid training set file.");
            line = sr.ReadLine();
            if (!line.StartsWith(GENERATED_AT))
                throw new IOException("This isn't a valid training set file.");

            for (int i = 0; i < 3; i++)
            {
                line = readNextSignificantLine(sr);
                String[] strArr = line.Split(':');
                if (strArr[0].Trim().Equals(NO_PATTERNS))
                    nbPatterns = int.Parse(strArr[1].Trim(), cultureInfo);
                else if (strArr[0].Trim().Equals(NO_INPUTS))
                    nbInputUnit = int.Parse(strArr[1].Trim(), cultureInfo);
                else if (strArr[0].Trim().Equals(NO_OUTPUTS))
                    nbOutputUnit = int.Parse(strArr[1].Trim(), cultureInfo);
                else
                    throw new IOException("Expected a different value");
            }
            return nbPatterns;
        }

        private string readNextSignificantLine(StreamReader sr)
        {
            String line = null;
            do
            {
                line = sr.ReadLine().Trim();
            } while (line.Length == 0 || line.StartsWith(COMMENT));
            return line;
        }

    }
}