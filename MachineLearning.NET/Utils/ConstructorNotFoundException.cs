using System;
using System.Runtime.Serialization;

namespace MachineLearning.NET.Utils
{
    internal class ConstructorNotFoundException : InstanceBuilderException
    {
		
        public ConstructorNotFoundException()
        {
        }
		
        public ConstructorNotFoundException(string message) : base(message)
        {
        }
		
        public ConstructorNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
		
        // needed for serialization
        protected ConstructorNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}