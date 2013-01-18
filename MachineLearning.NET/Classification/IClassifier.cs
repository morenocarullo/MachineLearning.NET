// 
// $Id: IClassifier.cs 21954 2010-11-03 08:27:35Z xpuser $
//  

using System.Collections.Generic;
using MachineLearning.NET.Data.Patterns;
using MachineLearning.NET.Evaluation;

namespace MachineLearning.NET.Classification
{
    /// <summary>
    /// This defines the interface of a general-purpose
    /// classifier.
    /// </summary>
    /// <typeparam name="P">the type that represents a single pattern</typeparam>
    /// <creator>Moreno Carullo</creator>
    /// <date>2008/07/15</date>
    public interface IClassifier<P>
        where P : IGenericPattern
    {
        /// <summary>
        /// Checks if the model has been trained.
        /// </summary>
        bool IsTrained { get; }

        /// <summary>
        /// This property says if the classifier predicts probabilities.
        /// </summary>
        bool HasProbabilities { get; }

        /// <summary>
        /// Train the classifier with a given set of patterns.
        /// </summary>
        /// <param name="patterns"></param>
        void Train(IEnumerable<P> patterns);

        /// <summary>
        /// Test the classifier with a given set of patterns.
        /// </summary>
        /// <param name="patterns"></param>
        /// <param name="qualityEvaluator"></param>
        void Test(IEnumerable<P> patterns, ComposedQualityEvaluator<int> qualityEvaluator);

        /// <summary>
        /// Discard all learned data.
        /// </summary>
        void Forget();

        /// <summary>
        /// Predict class-memberships of a given pattern.
        /// </summary>
        /// <param name="sparsePattern">the pattern to predict on</param>
        /// <returns></returns>
        IDictionary<int, float> Predict(P sparsePattern);

        /// <summary>
        /// Predict the winning class of a given pattern.
        /// </summary>
        /// <param name="spPattern">the pattern to predict on</param>
        /// <returns></returns>
        int PredictWinner(P spPattern);

        /// <summary>
        /// Save the model from a disk resource.
        /// </summary>
        /// <param name="modelFilePath"></param>
        void Save(string modelFilePath);

        /// <summary>
        /// Load the model from a disk resource.
        /// </summary>
        /// <param name="modelFilePath"></param>
        void Load(string modelFilePath);
    }
}