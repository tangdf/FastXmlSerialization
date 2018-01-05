using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FastXmlSerialization
{
    public static class MemberAccessorFactory
    {
        private static ConcurrentDictionary<PropertyInfo, IMemberAccessor> s_cache = new ConcurrentDictionary<PropertyInfo, IMemberAccessor>();

        public static IMemberAccessor Create(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));
            return s_cache.GetOrAdd(propertyInfo, _Create);
        }

        private static IMemberAccessor _Create(PropertyInfo propertyInfo)
        {
            return (IMemberAccessor) typeof(PropertyAccessor<,>).MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType)
                .New(propertyInfo);
        }
    }
}