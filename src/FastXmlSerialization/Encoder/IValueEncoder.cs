namespace FastXmlSerialization
{
    public interface IValueEncoder<TValue> : IValueEncoder
    {
        new TValue Decode(string value);

        string Encode(TValue value);
    }

    public interface IValueEncoder
    {
        bool Nullable { get; }

        object Decode(string value);

        string Encode(object value);
    }
}