using System;
using System.Collections;
using System.Reflection;
using System.Xml;

namespace FastXmlSerialization
{
    internal class CollectionPropertySerializer : BasePropertySerializer
    {
        public CollectionPropertySerializer(PropertyInfo propertyInfo) : base(propertyInfo)
        {
            this.MemberAccessor = MemberAccessorFactory.Create(this.PropertyInfo);

            this.ObjectSerializer = ObjectSerializerFactory.Create(this.PropertyInfo.PropertyType);
        }


        public IMemberAccessor MemberAccessor { get; }

        public IObjectSerializer ObjectSerializer { get; }


        public override string InputElementName
        {
            get { return "dataset"; }
        }

        public override string OutputElementName
        {
            get { return "sqldata"; }
        }


        public override void Write(XmlWriter xmlWriter, object obj)
        {
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            object value = MemberAccessor.GetValue(obj);

            if (value == null)
                return;

            ICollection collection = value as ICollection;

            if (collection == null) {
                throw new ArgumentException(string.Format("参数不是“{0}”类型。", typeof(ICollection).FullName), nameof(obj));
            }

            xmlWriter.WriteStartElement(this.InputElementName);

            this.ObjectSerializer.Write(xmlWriter, collection);

            xmlWriter.WriteEndElement();
        }

        public override void Read(XmlReader xmlReader, object obj)
        {
            if (xmlReader == null)
                throw new ArgumentNullException(nameof(xmlReader));
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            xmlReader.MoveToContent();

            object value = this.ObjectSerializer.Read(xmlReader);

            this.MemberAccessor.SetValue(obj, value);
        }
    }
}