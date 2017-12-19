using System;

namespace FastXmlSerialization
{
    internal class Int32ValueEncoder : BaseValueEncoder<int>
    {
        //public override bool Nullable
        //{
        //    get { return true; }
        //}

        public override int Decode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0; //空字符串 视同0
            return Convert.ToInt32(value);
        }

        public override string Encode(int value)
        {
            return value.ToString();
        }
    }
}