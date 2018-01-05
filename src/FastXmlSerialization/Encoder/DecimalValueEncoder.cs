using System;

namespace FastXmlSerialization
{
    internal class DecimalValueEncoder : BaseValueEncoder<decimal>
    {
        /// <summary>
        /// 是否为明细金额
        /// </summary>
        private readonly bool _isDetail;

        public DecimalValueEncoder(bool isDetail)
        {
            this._isDetail = isDetail;
        }

        //4.	文档内涉及金额字段，明细类金额为4位小数，结算类金额为2位小数

        //public override bool Nullable
        //{
        //    get { return false; }
        //}

        public override Decimal Decode(string value)
        {
            return Convert.ToDecimal(value);
        }

        public override string Encode(Decimal value)
        {
            //todo:增加单测
            string format = this._isDetail ? "0.0000" : "0.00";
            return value.ToString(format);
        }
    }
}