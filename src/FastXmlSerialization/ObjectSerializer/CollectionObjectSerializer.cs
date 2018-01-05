using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace FastXmlSerialization
{
    internal class CollectionObjectSerializer : IObjectSerializer
    {
        private readonly CollectionSerializer _collectionSerializer;

        public CollectionObjectSerializer(CollectionSerializer collectionSerializer)
        {
            _collectionSerializer = collectionSerializer;
        }

        public void Write(XmlWriter xmlWriter, object value)
        {
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            xmlWriter.WriteStartElement("dataset");

            _collectionSerializer.Write(xmlWriter, value);

            xmlWriter.WriteEndElement();
        }

        public object Read(XmlReader xmlReader)
        {
            if (xmlReader == null)
                throw new ArgumentNullException(nameof(xmlReader));
            xmlReader.MoveToContent();


            while (xmlReader.NodeType != XmlNodeType.EndElement && xmlReader.NodeType != XmlNodeType.None) {
                if (xmlReader.IsStartElement("sqldata")) {
                    if (xmlReader.IsEmptyElement) {
                        xmlReader.Skip();
                        xmlReader.MoveToContent();
                        continue;
                    }
                    xmlReader.ReadStartElement();
                    xmlReader.MoveToContent();

                    return _collectionSerializer.Read(xmlReader);
                }

                else {
                    throw new XmlSerializeException("错误的xml格式文档,集合必须以“sqldata”不根节点。");
                }
            }
            return null;
        }
    }
}