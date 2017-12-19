using System.Xml;

namespace FastXmlSerialization
{
    public interface IObjectSerializer
    {
        void Write(XmlWriter xmlWriter, object value);
        object Read(XmlReader xmlReader);
    }
}