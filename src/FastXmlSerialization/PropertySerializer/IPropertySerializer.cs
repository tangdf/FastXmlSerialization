using System.Xml;

namespace FastXmlSerialization
{
    public interface IPropertySerializer
    {
        string InputElementName { get; }

        string OutputElementName { get; }

        void Write(XmlWriter xmlWriter, object value);

        void Read(XmlReader xmlReader, object obj);
    }
}