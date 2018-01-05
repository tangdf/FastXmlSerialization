using System;
using System.IO;
using System.Text;
using System.Xml;

namespace FastXmlSerialization
{
    public sealed class ObjectXmlSerializer
    {
        public string Serialize(object obj)
        {
            StringBuilder stringBuilder = new StringBuilder(1024);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;

            using (StringWriter stringWriter = new GBKStringWriter(stringBuilder))

            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings)) {
                xmlWriter.WriteStartDocument(true);
                xmlWriter.WriteStartElement("input");
                if (obj != null) {
                    Serialize(xmlWriter, obj);
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
            return stringBuilder.ToString();
        }

        private void Serialize(XmlWriter xmlWriter, object obj)
        {
            IObjectSerializer serializer = CreateObjectSerializer(obj.GetType());
            serializer.Write(xmlWriter, obj);
        }

        private static IObjectSerializer CreateObjectSerializer(Type targetType)
        {
            IObjectSerializer objectSerializer = ObjectSerializerFactory.Create(targetType);
            if (objectSerializer is CollectionSerializer collectionSerializer) {
                return new CollectionObjectSerializer(collectionSerializer);
            }
            return objectSerializer;
        }

        public T Deserialize<T>(string xml)
        {
            return (T) Deserialize(xml, typeof(T));
        }

        public object Deserialize(string xml, Type targetType)
        {
            if (xml == null)
                throw new ArgumentNullException(nameof(xml));
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            using (StringReader stringReader = new StringReader(xml))

            using (XmlReader xmlReader = XmlReader.Create(stringReader)) {
                if (xmlReader.MoveToContent() == XmlNodeType.Element) {
                    while (xmlReader.NodeType != XmlNodeType.EndElement && xmlReader.NodeType != XmlNodeType.None) {
                        if (xmlReader.IsStartElement("output")) {
                            if (xmlReader.IsEmptyElement) {
                                xmlReader.Skip();
                                xmlReader.MoveToContent();
                                continue;
                            }
                            xmlReader.ReadStartElement();
                            xmlReader.MoveToContent();
                            return Deserialize(xmlReader, targetType);
                        }
                        throw new XmlSerializeException("错误的xml格式文档，文档不是以“output”为根节点。");
                    }
                }
            }
            return null;
        }

        private object Deserialize(XmlReader xmlReader, Type targetType)
        {
            IObjectSerializer serializer = CreateObjectSerializer(targetType);
            //serializer.Write(xmlWriter, obj);
            return serializer.Read(xmlReader);
        }
    }
}