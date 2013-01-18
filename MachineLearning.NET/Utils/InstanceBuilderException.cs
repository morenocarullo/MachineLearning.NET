using System;
using System.Runtime.Serialization;

namespace MachineLearning.NET.Utils
{
    internal class InstanceBuilderException : Exception, ISerializable
    {
        public InstanceBuilderException() : base()
        {
        }
		
        public InstanceBuilderException(string message) : base(message)
        {
        }
		
        public InstanceBuilderException(string message, Exception inner) : base(message, inner)
        {
        }
		
        // needed for serialization
        protected InstanceBuilderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}