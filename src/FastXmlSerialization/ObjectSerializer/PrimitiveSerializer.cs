using System;
using System.Xml;

namespace FastXmlSerialization
{
    internal class PrimitiveSerializer<TValue> : IObjectSerializer
    {
        private  IValueEncoder<TValue> _valueEncoder;

        public PrimitiveSerializer()
        {
            
        }

        public virtual IValueEncoder<TValue> ValueEncoder
        {
            get
            {
                if (this._valueEncoder == null)
                {
                    this._valueEncoder = this.CreateValueEncoder();

                }
               return this._valueEncoder;
            }
        }


        protected virtual IValueEncoder<TValue> CreateValueEncoder()
        {
           return  ValueEncoderFactory.Create<TValue>();
        }
   

        public void Write(XmlWriter xmlWriter, TValue value)
        {
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));

            string stringValue = this.ValueEncoder.Encode(value);
            if (this.ValueEncoder.Nullable == false && string.IsNullOrEmpty(stringValue))
                throw new XmlSerializeException(string.Format("“{0}”类型不能为空值。", typeof(TValue)));

            xmlWriter.WriteValue(stringValue ?? string.Empty);
            //这写入空字符串是为了产生非自闭合的元素 <input></input> not <input />
        }

        void IObjectSerializer.Write(XmlWriter xmlWriter, object value)
        {
            this.Write(xmlWriter, (TValue) value);
        }


        public TValue Read(XmlReader xmlReader)
        {
            if (xmlReader == null)
                throw new ArgumentNullException(nameof(xmlReader));

            var value = xmlReader.NodeType==XmlNodeType.Text  ? xmlReader.ReadContentAsString(): xmlReader.ReadElementContentAsString();
            if (this.ValueEncoder.Nullable == false && string.IsNullOrEmpty(value))
                throw new XmlSerializeException(string.Format("“{0}”类型不能为空值。", typeof(TValue)));
            return this.ValueEncoder.Decode(value);
        }

        object IObjectSerializer.Read(XmlReader xmlReader)
        {
            return this.Read(xmlReader);
        }
    }


    internal abstract class DecimalSerializer<TValue> : PrimitiveSerializer<TValue>
    {

        /// <summary>
        /// 是否为明细金额
        /// </summary>

        public DecimalSerializer(bool isDetail)
        {
            this.IsDetail = isDetail;
        }
        protected  bool IsDetail { get; }

        protected override IValueEncoder<TValue> CreateValueEncoder()
        {
            if(typeof(TValue)==typeof(decimal))
                return (IValueEncoder<TValue>)new DecimalValueEncoder(this.IsDetail);

             if(typeof(TValue)==typeof(decimal?))
                return (IValueEncoder<TValue>)new NullableValueEncoder<decimal>(new DecimalValueEncoder(this.IsDetail));

            throw new InvalidOperationException(string.Format("不支持数据类型“{0}”。",typeof(TValue)));
        }
    }
}