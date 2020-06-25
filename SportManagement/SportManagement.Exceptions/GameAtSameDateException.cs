using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SportManagement.Exceptions
{
    [Serializable]
    public class GameAtSameDateException : Exception
    {
        public GameAtSameDateException()
        {
        }

        public GameAtSameDateException(string message) : base(message)
        {
        }

        public GameAtSameDateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GameAtSameDateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
