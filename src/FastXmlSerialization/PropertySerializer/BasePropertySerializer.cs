using System;
using System.Reflection;
using System.Xml;

namespace FastXmlSerialization
{
    internal abstract class BasePropertySerializer : IPropertySerializer
    {
        protected BasePropertySerializer(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));
            this.PropertyInfo = propertyInfo;
        }

        protected PropertyInfo PropertyInfo { get; }

        public virtual string InputElementName
        {
            //节点全是小写。
            get { return this.PropertyInfo.Name.ToLowerInvariant(); }
        }

        public virtual string OutputElementName
        {
            get { return this.InputElementName; }
        }


        public abstract void Write(XmlWriter xmlWriter, object value);

        public abstract void Read(XmlReader xmlReader, object obj);
    }
}