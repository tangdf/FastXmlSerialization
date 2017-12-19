using System;
using System.Collections.Generic;
using System.Reflection;

namespace FastXmlSerialization
{
    internal static class PrimitiveSerializerFactory
    {
        private static Dictionary<Type, IObjectSerializer> s_cache = new Dictionary<Type, IObjectSerializer> {
            { typeof(string), new PrimitiveSerializer<string>() },
            { typeof(Int32), new PrimitiveSerializer<Int32>() },
            { typeof(Int64), new PrimitiveSerializer<Int64>() },
            { typeof(DateTime), new PrimitiveSerializer<DateTime>() },
        };


        public static IObjectSerializer Create(Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));


            Type underlyingType = Nullable.GetUnderlyingType(targetType);

            if (underlyingType == null) {
                underlyingType = targetType;
            }

            IObjectSerializer objectSerializer;
            if (s_cache.TryGetValue(underlyingType, out objectSerializer) == false)
                throw new InvalidOperationException(string.Format("不支持序列化类型“{0}”。", underlyingType.FullName));

            return objectSerializer;
        }
    }
}