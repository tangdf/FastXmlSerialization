using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace FastXmlSerialization
{
    internal static class ObjectSerializerFactory
    {
        private static readonly ConcurrentDictionary<Type, IObjectSerializer> s_typeMap = new ConcurrentDictionary<Type, IObjectSerializer>();

        public static IObjectSerializer Create(Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));


            return s_typeMap.GetOrAdd(targetType, _Create);
        }



        private static IObjectSerializer _Create(Type targetType)
        {
            if (targetType.IsCollection()) {
                return new CollectionSerializer(targetType);
            }
            else if (targetType.IsComplexType()) {
                return new ComplexSerializer(targetType);
            }
            else {
                return PrimitiveSerializerFactory.Create(targetType);
            }
        }
    }
}