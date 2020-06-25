using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SportManagement.Exceptions
{
    [Serializable]
    public class EmailFormatException : Exception
    {
        public EmailFormatException()
        {
        }

        public EmailFormatException(string message) : base(message)
        {
        }

        public EmailFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmailFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
