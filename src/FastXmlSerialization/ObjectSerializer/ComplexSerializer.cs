using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;

namespace FastXmlSerialization
{
    internal class ComplexSerializer : IObjectSerializer
    {
        //private readonly object _obj;
        //private static ConcurrentDictionary<Type, InternalXmlSerializer> s_cache = new ConcurrentDictionary<Type, InternalXmlSerializer>();

        private readonly Dictionary<string, IPropertySerializer> _serializeMap = new Dictionary<string, IPropertySerializer>(StringComparer.Ordinal);
        private readonly Dictionary<string, IPropertySerializer> _deserializeMap = new Dictionary<string, IPropertySerializer>(StringComparer.Ordinal);

        public ComplexSerializer(Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));
            this.TargetType = targetType;
            this.Init();
        }


        private void Init()
        {
            IEnumerable<PropertyInfo> properties = this.TargetType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(prop =>
                prop.GetIndexParameters().Length == 0 && prop.GetGetMethod(true) != null && prop.GetSetMethod(true) != null);

            foreach (PropertyInfo propertyInfo in properties) {
                if (propertyInfo.GetAttribute<FastXmlIgnoreAttribute>() == null) {

                    IPropertySerializer serializer = PropertySerializerFactory.Create(propertyInfo);
                  
                    _serializeMap.Add(serializer.InputElementName, serializer);
                    _deserializeMap.Add(serializer.OutputElementName, serializer);
                }
            }
        }


        public Type TargetType { get; }


        public void Write(XmlWriter xmlWriter, object obj)
        {
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (_serializeMap.Count > 0) {
                foreach (IPropertySerializer propertySerializer in _serializeMap.Values) {
                    propertySerializer.Write(xmlWriter, obj);
                }
            }
            else {
                xmlWriter.WriteValue(string.Empty);
                //这写入空字符串是为了产生非自闭合的元素 <input></input>  not <input />
            }
        }

        public object Read(XmlReader xmlReader)
        {
            if (xmlReader == null)
                throw new ArgumentNullException(nameof(xmlReader));

            object result = null;
            xmlReader.MoveToContent();
            while (xmlReader.NodeType != XmlNodeType.EndElement && xmlReader.NodeType != XmlNodeType.None) {

                IPropertySerializer propertySerializer;
                if (string.IsNullOrEmpty(xmlReader.Name)==false && this._deserializeMap.TryGetValue(xmlReader.Name, out propertySerializer))
                {
                    if (result == null)
                    {
                        result = this.TargetType.New();
                    }
                    if (xmlReader.IsEmptyElement)
                    {
                        propertySerializer.Read(xmlReader, result);
                        //xmlReader.Skip();
                        //xmlReader.MoveToContent();
                        continue;
                    }
                    else {
                        xmlReader.ReadStartElement();
                        xmlReader.MoveToContent();
                        propertySerializer.Read(xmlReader, result);
                    }
                }
                xmlReader.Skip();
            }
            //while (xmlReader.MoveToContent() == XmlNodeType.Element) {
            //    while (xmlReader.NodeType != XmlNodeType.EndElement && xmlReader.NodeType != XmlNodeType.None)
            //    {
            //    }
            //(xmlReader.MoveToContent())
            //while (dept == xmlReader.Depth &&  xmlReader.MoveToContent()==XmlNodeType.Element) {
            //    var name = xmlReader.Name;
            //    IPropertySerializer propertySerializer;
            //    if (this._serializers.TryGetValue(name, out propertySerializer)) {
            //        if (result == null) {
            //            result = this.TargetType.New();
            //        }
            //        propertySerializer.Read(xmlReader, result);
            //    }
            //}
            return result;
        }

        //public void Read(XmlReader xmlReader,object obj)
        //{
        //    if (obj == null)
        //        throw new ArgumentNullException(nameof(obj));
        //    var depth = xmlReader.Depth;
        //    while (depth== xmlReader.Depth &&  xmlReader.Read()) {
        //        if (xmlReader.NodeType == XmlNodeType.Element) {
        //            //xmlReader.Value
        //        }
        //    }
        //}
    }
}