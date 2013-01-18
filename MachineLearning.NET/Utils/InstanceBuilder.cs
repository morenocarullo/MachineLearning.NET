using System;
using System.Collections.Generic;
using System.Reflection;

namespace MachineLearning.NET.Utils
{
    internal class InstanceBuilder<E>
    {
        public static E BuildInstance<E>(Type objectType, params KeyValuePair<Type, object>[] constructorParameters)
        {
            //build the array with constructor types
            //for retrieving the right constructor
            var inputTypes = new Type[constructorParameters.Length];
            for (var i = 0; i < inputTypes.Length; i++) {
                inputTypes[i] = constructorParameters[i].Key;
            }
            //try to get the right constructor
            var constructor = objectType.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public, null,
                CallingConventions.HasThis, inputTypes, null);
            if (constructor == null) {
                throw new ConstructorNotFoundException("InstanceBuilder: there isn't a constructor with these arguments");
            }
            //build the array with the constructor values
            var parameters = new object[constructorParameters.Length];
            for (var i = 0; i < constructorParameters.Length; i++) {
                parameters[i] = constructorParameters[i].Value;
            }
            //finally build the new instance
            return (E) constructor.Invoke(parameters);
        }
    }
}