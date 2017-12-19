using System;
using System.Runtime.Serialization;

namespace FastXmlSerialization
{
    [Serializable]
    public class XmlSerializeException : Exception
    {
        public XmlSerializeException()
        {
        }

        public XmlSerializeException(string message) : base(message)
        {
        }

        public XmlSerializeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected XmlSerializeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}