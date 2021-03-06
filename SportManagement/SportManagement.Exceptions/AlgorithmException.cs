﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SportManagement.Exceptions
{
    [Serializable]
    public class AlgorithmException : Exception
    {
        public AlgorithmException()
        {
        }

        public AlgorithmException(string message) : base(message)
        {
        }

        public AlgorithmException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AlgorithmException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
