using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SportManagement.Exceptions
{
    [Serializable]
    public class NotUniqueException : Exception
    {

        public NotUniqueException()
        {
        }

        public NotUniqueException(string message) : base(message)
        {
        }

        public NotUniqueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotUniqueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
