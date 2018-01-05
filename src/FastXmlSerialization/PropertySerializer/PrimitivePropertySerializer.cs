using System;
using System.Reflection;
using System.Xml;

namespace FastXmlSerialization
{
    internal class PrimitivePropertySerializer<TValue> : BasePropertySerializer
    {
        public PrimitivePropertySerializer(PropertyInfo propertyInfo) : base(propertyInfo)
        {
            this.MemberAccessor = (IMemberAccessor<TValue>) MemberAccessorFactory.Create(this.PropertyInfo);

            this.ObjectSerializer =
                CreateObjectSerializer(); //new PrimitiveSerializer<TValue>();//(PrimitiveSerializer<TValue>)PrimitiveSerializerFactory.Create(propertyInfo);
        }


        public IMemberAccessor<TValue> MemberAccessor { get; }

        public PrimitiveSerializer<TValue> ObjectSerializer { get; }


        private PrimitiveSerializer<TValue> CreateObjectSerializer()
        {
            Type targetType = this.PropertyInfo.PropertyType;

            if (targetType == typeof(Decimal) || targetType == typeof(Decimal?)) {
                AmountAttribute amountAttribute = this.PropertyInfo.GetAttribute<AmountAttribute>();

                if (amountAttribute == null) {
                    throw new InvalidOperationException(string.Format("在金额属性上面必须使用“{0}”标记。", typeof(AmountAttribute).FullName));
                }

                if (targetType == typeof(Decimal))
                    return (PrimitiveSerializer<TValue>) (object) new AmountSerializer(amountAttribute.IsDetail);

                else
                    return (PrimitiveSerializer<TValue>) (object) new NullableAmountSerializer(amountAttribute.IsDetail);
            }

            return new PrimitiveSerializer<TValue>();
        }


        public override void Read(XmlReader xmlReader, object obj)
        {
            if (xmlReader == null)
                throw new ArgumentNullException(nameof(xmlReader));
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            xmlReader.MoveToContent();
            while (xmlReader.NodeType != XmlNodeType.EndElement && xmlReader.NodeType != XmlNodeType.None) {
                xmlReader.MoveToContent();

                TValue value = this.ObjectSerializer.Read(xmlReader);
                this.MemberAccessor.SetValue(obj, value);
                return;
            }
        }

        public override void Write(XmlWriter xmlWriter, object obj)
        {
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            TValue value = (TValue) this.MemberAccessor.GetValue(obj);

            xmlWriter.WriteStartElement(this.InputElementName);
            this.ObjectSerializer.Write(xmlWriter, value);
            xmlWriter.WriteEndElement();
        }
    }
}