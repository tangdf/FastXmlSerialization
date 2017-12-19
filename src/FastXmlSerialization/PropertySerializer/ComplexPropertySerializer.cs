using System;
using System.Reflection;
using System.Xml;

namespace FastXmlSerialization
{
    internal class ComplexPropertySerializer : BasePropertySerializer
    {


        public ComplexPropertySerializer(PropertyInfo propertyInfo) : base(propertyInfo)
        {

            this.MemberAccessor = MemberAccessorFactory.Create(this.PropertyInfo);

            this.ObjectSerializer = ObjectSerializerFactory.Create(this.PropertyInfo.PropertyType);
        }

        public IMemberAccessor MemberAccessor { get; }


        public IObjectSerializer ObjectSerializer { get; }


        public override void Write(XmlWriter xmlWriter, object obj)
        {
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            xmlWriter.WriteStartElement(this.InputElementName);

            object value = MemberAccessor.GetValue(obj);

            if (value != null) {

                this.ObjectSerializer.Write(xmlWriter, value);
            }

            xmlWriter.WriteEndElement();
       
        }

        public override void Read(XmlReader xmlReader, object obj)
        {
            if (xmlReader == null)
                throw new ArgumentNullException(nameof(xmlReader));
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            object value = this.ObjectSerializer.Read(xmlReader);
            this.MemberAccessor.SetValue(obj, value);
        }
    }
}