namespace FastXmlSerialization
{
    public abstract class BaseValueEncoder<TValue> : IValueEncoder<TValue>, IValueEncoder
    {
        public virtual bool Nullable { get { return true; } }

        public abstract TValue Decode(string value);


        public abstract string Encode(TValue value);


        string IValueEncoder.Encode(object value)
        {
            return this.Encode((TValue) value);
        }

        object IValueEncoder.Decode(string value)
        {
            return this.Decode(value);
        }
    }
}