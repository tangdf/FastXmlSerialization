namespace FastXmlSerialization
{
    internal class StringValueEncoder : BaseValueEncoder<string>
    {
        public override bool Nullable
        {
            get { return true; }
        }

        public override string Decode(string value)
        {
            return string.IsNullOrEmpty(value)?null:value;
        }

        public override string Encode(string value)
        {
            return value;
        }
    }
}