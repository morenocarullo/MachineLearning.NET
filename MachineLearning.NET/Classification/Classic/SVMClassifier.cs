using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Globalization;
using System.Linq;
using log4net;
using MachineLearning.NET.Data.IO;
using MachineLearning.NET.Data.Patterns;
using SVM;

namespace MachineLearning.NET.Classification.Classic
{
    [Classifier(Name="svm")]
    public class SVMClassifier : BaseClassifier<SparsePattern>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SVMClassifier));
        private static readonly NumberFormatInfo s_numberFormat = CultureInfo.InvariantCulture.NumberFormat;
        private readonly IDictionary<string, string> m_configuration;

        private Model m_model;

        private RangeTransform m_rangeTransform;

        public SVMClassifier(IDictionary<string, string> configuration)
        {
            m_configuration = configuration;

            HasRangeTransform = m_configuration.ContainsKey("rt") ? bool.Parse(m_configuration["rt"]) : false;
        }

        public override bool IsTrained
        {
            get { return m_model != null; }
        }

        private bool HasRangeTransform
        {
            get; set;
        }

        public override bool HasProbabilities
        {
            get { return m_model.Parameter.Probability; }
        }

        public override void Train(IEnumerable<SparsePattern> patterns)
        {
            var trainProbabilities = m_configuration.ContainsKey("prob");

            var sparseTrainFile = Path.GetTempFileName();

            try
            {
                using(var dw = DatasetWriterFactory.GetDatasetWriter<SparsePattern>(sparseTrainFile))
                {
                    foreach(var pattern in patterns.Where(p=>p.Features.Count>0))
                    {
                        dw.Write(pattern);
                    }
                }

                Train(sparseTrainFile, trainProbabilities);
            }
            finally
            {
                if(File.Exists(sparseTrainFile))
                {
                    File.Delete(sparseTrainFile);
                }
            }
        }

        private void Train(string sSparseTrainFile, bool bTrainProbabilities)
        {
            // 1. Reading configuration
            var parameters = new Parameter { SvmType = SvmType.C_SVC, Probability = bTrainProbabilities };
            var svmType = m_configuration.ContainsKey("type") ? m_configuration["type"] : "linear";
            var gamma = m_configuration.ContainsKey("gamma") ? double.Parse(m_configuration["gamma"], s_numberFormat) : 1.0;
            var auto = m_configuration.ContainsKey("auto") ? bool.Parse(m_configuration["auto"]) : false;
            var C = m_configuration.ContainsKey("C") ? double.Parse(m_configuration["C"], s_numberFormat) : 1;

            switch (svmType)
            {
                case "rbf":
                    parameters.KernelType = KernelType.RBF;
                    parameters.Gamma = gamma;
                    break;
                case "poly":
                    parameters.KernelType = KernelType.POLY;
                    parameters.Gamma = gamma;
                    break;
                case "sigmoid":
                    parameters.KernelType = KernelType.SIGMOID;
                    parameters.Gamma = gamma;
                    break;
                default:
                    parameters.KernelType = KernelType.LINEAR;
                    break;
            }

            if(parameters.Probability)
            {
                log.InfoFormat("NOTE: Using probability estimation.");
            }

            // 2. Read training and rescale
            var train = Problem.Read(sSparseTrainFile);
            log.InfoFormat("SVM: Read {0} patterns in TrS", train.Count);
            if(HasRangeTransform)
            {
                log.InfoFormat("Scaling Train...");
                m_rangeTransform = RangeTransform.Compute(train);
                train = m_rangeTransform.Scale(train);
            }

            // 3. Auto-search parameters
            if(auto)
            {
                log.InfoFormat("Tuning C and gamma parameters with grid search...");
                ParameterSelection.Grid(train, parameters, sSparseTrainFile+"-svmparams-search.txt", out C, out gamma);
                log.InfoFormat("Done. Kernel = {0}, Gamma = {1}, C = {2}", parameters.KernelType, parameters.Gamma, parameters.C);
            }
			
            // 4. Train the SVM
            log.InfoFormat("Training SVM with kernel = {0}, gamma = {1}, C = {2}", svmType, gamma, C);
            parameters.Gamma = gamma;
            parameters.C = C;
            m_model = Training.Train(train, parameters);
        }
		
        public override void Forget()
        {
            m_model = null;
        }

        public override int PredictWinner (SparsePattern spPattern)
        {
            Node[] patternForSVM = GetPatternForSVM(spPattern);

            return (int)Prediction.Predict(m_model, patternForSVM);
        }

        public override IDictionary<int, float> Predict(SparsePattern sparsePattern)
        {
            if(HasProbabilities)
            {
                var patternForSVM = GetPatternForSVM(sparsePattern);

                var svmPrediction = Prediction.PredictProbability(m_model, patternForSVM);

                var prediction = new Dictionary<int, float>();

                for(var classId=0; classId<m_model.ClassLabels.Length;classId++)
                {
                    var classLabel = m_model.ClassLabels[classId];
                    prediction.Add(classLabel, (float)svmPrediction[classId]);
                }

                return prediction;
            }

            IDictionary<int, float> dtPrediction = new Dictionary<int, float>();
            var nWinnerClass = PredictWinner(sparsePattern);
            dtPrediction.Add(nWinnerClass, 1);
            return dtPrediction;
        }

        private Node[] GetPatternForSVM(SparsePattern spPattern)
        {
            var patternForSVM = new Node[spPattern.Features.Count];
            var ft = 0;

            foreach (var nFeature in spPattern.Features.Keys)
            {
                patternForSVM[ft++] = new Node(nFeature, spPattern.Features[nFeature]);
            }

            if (HasRangeTransform)
            {
                patternForSVM = m_rangeTransform.Transform(patternForSVM);
            }
            return patternForSVM;
        }

        #region Load & Save methods

        public override void Save(string sModelFilePath)
        {
            using(var fileStream = new FileStream(sModelFilePath, FileMode.Create))
            using(var gzipStream = new GZipStream(fileStream, CompressionMode.Compress))
            {
                Model.Write(gzipStream, m_model);
            }

            if (HasRangeTransform)
            {
                using (var fileStream = new FileStream(sModelFilePath+".rt", FileMode.Create))
                using (var gzipStream = new GZipStream(fileStream, CompressionMode.Compress))
                {
                    RangeTransform.Write(gzipStream, m_rangeTransform);
                }
            }
        }
		
        public override void Load(string sModelFilePath)
        {
            using(var fileStream = new FileStream(sModelFilePath, FileMode.Open))
            using(var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
            {		
                m_model = Model.Read(gzipStream);
            }

            if (HasRangeTransform)
            {
                var rangeTransformFilePath = sModelFilePath + ".rt";
                if (!File.Exists(rangeTransformFilePath))
                {
                    throw new Exception(string.Format("The specified model requires a RangeTransform file ({0})", rangeTransformFilePath));
                }

                using (var fileStream = new FileStream(rangeTransformFilePath, FileMode.Open))
                using (var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
                {
                    m_rangeTransform = RangeTransform.Read(gzipStream);
                }
            }
        }

        #endregion
    }
}