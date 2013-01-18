//
// $Id: ClassifierFactory.cs 21954 2010-11-03 08:27:35Z xpuser $
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MachineLearning.NET.Data.Patterns;
using MachineLearning.NET.Utils;

namespace MachineLearning.NET.Classification
{
    public static class ClassifierFactory
    {		
        /// <summary>
        /// Factory method that returns an instance of the desired classifier.
        /// </summary>
        /// <param name="modelAndConfiguration">the model name (and optionaly a configuration after ":")</param>
        /// <returns>a classifier instance</returns>
        public static IClassifier<SparsePattern> GetClassicClassifier(string modelAndConfiguration)
        {
            return GetClassifier<SparsePattern>(modelAndConfiguration);
        }

        /// <summary>
        /// Get a classifier that is suitable in the Multi-Instance learning scenario.
        /// </summary>
        /// <param name="modelAndConfiguration"></param>
        /// <returns></returns>
        public static IClassifier<SparseBag> GetMultiInstanceClassifier(string modelAndConfiguration)
        {
            return GetClassifier<SparseBag>(modelAndConfiguration);
        }

        /// <summary>
        /// Get a classifier for a specific kind of pattern.
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <param name="modelAndConfiguration"></param>
        /// <returns></returns>
        private static IClassifier<P> GetClassifier<P>(string modelAndConfiguration)
            where P:IGenericPattern
        {
            var parts = modelAndConfiguration.Split(':');
            var learningModel = parts[0];
            
            var searchedDistance =
                (from t in Assembly.GetExecutingAssembly().GetTypes()
                 let attributes = t.GetCustomAttributes(typeof(ClassifierAttribute), false)
                 where
                     t.GetInterfaces().Contains(typeof(IClassifier<P>)) &&
                     attributes != null &&
                     attributes.Any(o => ((ClassifierAttribute)o).Name == learningModel)
                 select t).FirstOrDefault();

            if (searchedDistance == null)
            {
                throw new ArgumentException(string.Format("The specified classifier {0} doesn't exist", learningModel));
            }

            return InstanceBuilder<IClassifier<P>>.BuildInstance<IClassifier<P>>(
                searchedDistance,
                new KeyValuePair<Type, object>(
                    typeof(IDictionary<string, string>),
                    modelAndConfiguration.GetParamsFromConfigString()));
        }
    }
}