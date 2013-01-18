using System;
using System.Runtime.Serialization;

namespace MachineLearning.NET.Classification.MultiInstance
{
    public class NotBinaryProblemException : Exception
    {
        public NotBinaryProblemException() : base("The problem is not binary")
        {
        }

        public NotBinaryProblemException(string message) : base(message)
        {
        }

        public NotBinaryProblemException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotBinaryProblemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}