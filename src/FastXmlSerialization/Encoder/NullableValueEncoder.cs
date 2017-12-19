using System;

namespace FastXmlSerialization
{
    internal class NullableValueEncoder<TValue> : BaseValueEncoder<TValue?> where TValue : struct
    {
        private IValueEncoder<TValue> _valueEncoder;

        public override bool Nullable
        {
            get { return true; }
        }

        public NullableValueEncoder(IValueEncoder<TValue> valueEncoder)
        {
            if (valueEncoder == null)
                throw new ArgumentNullException(nameof(valueEncoder));

            _valueEncoder = valueEncoder;
        }

        public override TValue? Decode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            return _valueEncoder.Decode(value);
        }

        public override string Encode(TValue? value)
        {
            if (value.HasValue == false)
                return null;
            return _valueEncoder.Encode(value.Value);
        }
    }
}