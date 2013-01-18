// TypeNotFoundException.cs created with MonoDevelop
//da Alessandro Cavallaro alle 18:55 26/11/2008
//
//

using System;
using System.Runtime.Serialization;

namespace MachineLearning.NET.Utils
{
    internal class TypeNotFoundException : InstanceBuilderException
    {
		
        public TypeNotFoundException() : base()
        {
        }
		
        public TypeNotFoundException(string message) : base(message)
        {
        }
		
        public TypeNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
		
        // needed for serialization
        protected TypeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}