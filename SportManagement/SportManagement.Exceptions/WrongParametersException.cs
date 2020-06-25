using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SportManagement.Exceptions
{
    [Serializable]
    public class WrongParametersException : Exception
    {
        public WrongParametersException()
        {
        }

        public WrongParametersException(string message) : base(message)
        {
        }

        public WrongParametersException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongParametersException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
