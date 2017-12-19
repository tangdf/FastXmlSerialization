using System;
using System.Collections.Generic;
using System.Reflection;

namespace FastXmlSerialization
{
    internal static class ValueEncoderFactory
    {
        private static Dictionary<Type, object> s_cache = new Dictionary<Type, object> {
            { typeof(string), (object) new StringValueEncoder() },
            { typeof(Int32), (object) new Int32ValueEncoder() },
            { typeof(Int64), (object) new Int64ValueEncoder() },
            { typeof(DateTime), (object) new DateTimeValueEncoder() },
            //{ typeof(Decimal), (object) new DecimalValueEncoder() }
        };

        private static readonly MethodInfo s_createNullableMethod =
            typeof(ValueEncoderFactory).GetMethod(nameof(CreateNullable), BindingFlags.Static | BindingFlags.NonPublic);


        public static IValueEncoder<TValue> Create<TValue>()
        {
            Type targetType = typeof(TValue);

            Type underlyingType = Nullable.GetUnderlyingType(targetType);
            bool isNullType = underlyingType != null;
            if (underlyingType == null) {
                underlyingType = targetType;
            }

            object valueEncoder;
            if (s_cache.TryGetValue(underlyingType, out valueEncoder) == false)
                throw new InvalidOperationException(string.Format("不支持序列化类型“{0}”。", underlyingType.FullName));

            if (isNullType)
                return (IValueEncoder<TValue>) s_createNullableMethod.MakeGenericMethod(underlyingType).Invoke(null, new object[] { valueEncoder });

            return (IValueEncoder<TValue>) valueEncoder;
        }

        private static IValueEncoder<TValue?> CreateNullable<TValue>(IValueEncoder<TValue> valueEncoder) where TValue : struct
        {
            return new NullableValueEncoder<TValue>(valueEncoder);
        }
    }
}