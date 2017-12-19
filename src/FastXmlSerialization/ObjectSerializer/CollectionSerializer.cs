using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace FastXmlSerialization
{
    internal class CollectionSerializer : IObjectSerializer
    {
       
        public Type TargetType { get; }


        public Type ElementType { get; }

        public IObjectSerializer ElementObjectSerializer { get; }

        public CollectionSerializer(Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            this.TargetType = targetType;
            this.ElementType = this.TargetType.GetCollectionElemenType();
            this.ElementObjectSerializer = ObjectSerializerFactory.Create(this.ElementType);
        }

        public void Write(XmlWriter xmlWriter, object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));


            ICollection collection = (ICollection) value;

            Write(xmlWriter, collection);
        }



        public virtual void Write(XmlWriter xmlWriter, ICollection value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.Count > 0) {

             

                foreach (var item in value) {
                    if (item != null) {
                        xmlWriter.WriteStartElement("row");

                       
                        ElementObjectSerializer.Write(xmlWriter, item);

                        xmlWriter.WriteEndElement();
                    }
                }}
        }


        public ICollection Read(XmlReader xmlReader)
        {
            if (xmlReader == null)
                throw new ArgumentNullException(nameof(xmlReader));
            ICollectionWrapper result = null;
            if (xmlReader.MoveToContent() == XmlNodeType.Element) {
              
                 



                while (xmlReader.NodeType != XmlNodeType.EndElement && xmlReader.NodeType != XmlNodeType.None) {
                    if (xmlReader.IsStartElement("row")) {
                        if (result == null) {
                            result = CreateCollectionWrapper();
                        }
                        if (xmlReader.IsEmptyElement) {
                            xmlReader.Skip();
                            xmlReader.MoveToContent();
                            continue;
                        }
                        xmlReader.ReadStartElement();
                        xmlReader.MoveToContent();

                        var item = this.ElementObjectSerializer.Read(xmlReader);

                        result.Add(item);

                        xmlReader.ReadEndElement();
                      
                        xmlReader.MoveToContent();
                    }
                    else {

                        throw new XmlSerializeException("错误的xml格式文档,集合的元素必须以“row”不根节点。");
                    }

                }

            }
            return result == null ? null : result.Result;
        }


        private ICollectionWrapper CreateCollectionWrapper()
        {
          return   (ICollectionWrapper)typeof(CollectionWrapper<>).MakeGenericType(this.ElementType).New(this.TargetType.IsArray);
        }


        object IObjectSerializer.Read(XmlReader xmlReader)
        {
            return this.Read(xmlReader);

        }

      
    }


    internal interface ICollectionWrapper
    {
        void Add(object obj);
        ICollection Result { get; }
    }

    public class CollectionWrapper<T> : ICollectionWrapper
    {
        private readonly List<T> _result;

        public CollectionWrapper(bool isArray)
        {
            IsArray = isArray;
            _result = new List<T>();
        }

        public bool IsArray { get; }

        public void Add(object obj)
        {
            this._result.Add((T) obj);
        }

        public ICollection Result
        {
            get { return this.IsArray ?   (ICollection)this._result.ToArray() : this._result; }
        }
    }
}