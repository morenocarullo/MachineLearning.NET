using System;

namespace MachineLearning.NET.Data.Patterns
{
    /// <summary>
    /// Represents a  single pattern of the training set.
    /// 
    /// author: Ignazio
    /// </summary>
    public class SNNSPattern
    {
        #region properties

        /// <summary>
        /// Input pattern
        /// </summary>
        private double[] trainInput = null;
        public double[] Input
        {
            get { return trainInput; }
        }
        /// <summary>
        /// Output pattern
        /// </summary>
        private double[] trainOutput = null;
        public double[] Output
        {
            get { return trainOutput; }
        }
        /// <summary>
        /// Output pattern
        /// </summary>
        private string comment = "";
        public string Comment
        {
            get { return comment; }
        }
        #endregion

        /// <summary>
        /// Constructor used to load patterns from file.
        /// Gli array passati in input vengono copiati in campi privati della classe.
        /// </summary>
        /// <param name="inputs">array of double</param>
        /// <param name="outputs">array of double</param>
        public SNNSPattern(double[] inputs, double[] outputs)
        {
            trainInput = new double[inputs.Length];
            Array.Copy(inputs, trainInput, inputs.Length);

            trainOutput = new double[outputs.Length];
            Array.Copy(outputs, trainOutput, outputs.Length);
        }

        public SNNSPattern(double[] inputs, double[] outputs, string comment)
        {
            trainInput = new double[inputs.Length];
            Array.Copy(inputs, trainInput, inputs.Length);

            trainOutput = new double[outputs.Length];
            Array.Copy(outputs, trainOutput, outputs.Length);

            this.comment = comment;
        }

        public override string ToString()
        {
            string str = "[";
            foreach(double p in trainInput)
                str += p + ", ";
            str += "]; [";
            foreach(double p in trainOutput)
                str += p + ", ";
            return str + "]; ";
        }
    }
}