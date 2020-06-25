using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SportManagement.Exceptions
{
    [Serializable]
    public class NotExistsException : Exception
    {
        public NotExistsException()
        {
        }

        public NotExistsException(string message) : base(message)
        {
        }

        public NotExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
