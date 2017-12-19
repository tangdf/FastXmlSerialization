namespace FastXmlSerialization
{
    public interface IMemberAccessor
    {
        object GetValue(object obj);

        void SetValue(object obj, object value);
    }

    public interface IMemberAccessor<TValue>: IMemberAccessor
    {
        new TValue GetValue(object obj);

        void SetValue(object obj, TValue value);
    }

    public interface IMemberAccessor<in TDeclaring, TValue>: IMemberAccessor<TValue>
    {
        TValue GetValue(TDeclaring obj);

        void SetValue(TDeclaring obj, TValue value);
    }
}