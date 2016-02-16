using System;

namespace Mniak.Network
{
    [Serializable]
    public class SecureSocketException : Exception
    {
        public SecureSocketException() { }
        public SecureSocketException(string message) : base(message) { }
        public SecureSocketException(string message, Exception inner) : base(message, inner) { }
        protected SecureSocketException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
