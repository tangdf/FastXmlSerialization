using System;

namespace FastXmlSerialization
{
    internal class Int64ValueEncoder : BaseValueEncoder<long>
    {
        //public override bool Nullable
        //{
        //    get { return true; }
        //}

        public override Int64 Decode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0; //空字符串 视同0
            return Convert.ToInt64(value);
        }

        public override string Encode(Int64 value)
        {
            return value.ToString();
        }
    }
}