using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace FastXmlSerialization
{
    internal static class PropertySerializerFactory
    {
        private static readonly ConcurrentDictionary<PropertyInfo, IPropertySerializer> s_propertySerializers =
            new ConcurrentDictionary<PropertyInfo, IPropertySerializer>();

        private static Dictionary<Type, Func<PropertyInfo, IPropertySerializer>> s_PrimitiveMap =
            new Dictionary<Type, Func<PropertyInfo, IPropertySerializer>> {
                { typeof(string), propertyInfo => new PrimitivePropertySerializer<string>(propertyInfo) },
                { typeof(Int32), propertyInfo => new PrimitivePropertySerializer<Int32>(propertyInfo) },
                { typeof(Int64), propertyInfo => new PrimitivePropertySerializer<Int64>(propertyInfo) },
                { typeof(DateTime), propertyInfo => new PrimitivePropertySerializer<DateTime>(propertyInfo) },
                { typeof(Decimal), propertyInfo => new PrimitivePropertySerializer<Decimal>(propertyInfo) },
                { typeof(Int32?), propertyInfo => new PrimitivePropertySerializer<Int32?>(propertyInfo) },
                { typeof(Int64?), propertyInfo => new PrimitivePropertySerializer<Int64?>(propertyInfo) },
                { typeof(DateTime?), propertyInfo => new PrimitivePropertySerializer<DateTime?>(propertyInfo) },
                { typeof(Decimal?), propertyInfo => new PrimitivePropertySerializer<Decimal?>(propertyInfo) }
            };

        public static IPropertySerializer Create(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));
            return s_propertySerializers.GetOrAdd(propertyInfo, _Create);
        }

        private static IPropertySerializer _Create(PropertyInfo propertyInfo)
        {
            Type propertyType = propertyInfo.PropertyType;

            Type underlyingType = Nullable.GetUnderlyingType(propertyType);

            if (underlyingType == null) {
                underlyingType = propertyType;
            }

            if (underlyingType.IsCollection())
                return new CollectionPropertySerializer(propertyInfo);

            if (underlyingType.IsComplexType())
                return new ComplexPropertySerializer(propertyInfo);

            Func<PropertyInfo, IPropertySerializer> func;

            if (s_PrimitiveMap.TryGetValue(propertyType, out func) == false)
                throw new InvalidOperationException(string.Format("不支持序列化类型“{0}”。", underlyingType.FullName));

            return func.Invoke(propertyInfo);
        }
    }
}