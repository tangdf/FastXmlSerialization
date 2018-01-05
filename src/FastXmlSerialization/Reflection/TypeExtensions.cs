using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace FastXmlSerialization
{
    internal static class TypeExtensions
    {
        public static bool IsCollection(this Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));
            return typeof(ICollection).IsAssignableFrom(targetType);
        }


        public static TAttribute GetAttribute<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute
        {
            if (memberInfo == null)
                throw new ArgumentNullException(nameof(memberInfo));

            var array = memberInfo.GetCustomAttributes(typeof(TAttribute), true);

            if (array.Length == 0)
                return null;

            if (array.Length > 1)
                throw new InvalidOperationException(string.Format("成员的存在多个“{0}”标记。", typeof(TAttribute)));

            return (TAttribute) array[0];
        }

        public static bool IsComplexType(this Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));
            return TypeDescriptor.GetConverter(targetType).CanConvertFrom(typeof(string)) == false;
        }


        public static object New(this Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            return System.Activator.CreateInstance(targetType);
        }

        public static object New(this Type targetType, params object[] args)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            return System.Activator.CreateInstance(targetType, args);
        }

        public static Type GetCollectionElemenType(this Type collectionType)
        {
            if (collectionType == null)
                throw new ArgumentNullException(nameof(collectionType));

            var interfaceType = collectionType.GetInterfaces()
                .FirstOrDefault(item => item.IsGenericType && item.GetGenericTypeDefinition() == typeof(ICollection<>));

            if (interfaceType == null)
                throw new ArgumentException(string.Format("类型{0}未实现{1}泛型接口。", collectionType.FullName, typeof(ICollection<>).FullName));

            return interfaceType.GetGenericArguments()[0];
        }

        public static Func<TDeclaring, TValue> CreatePropertyGetter<TDeclaring, TValue>(MethodInfo getMethod)
        {
            if (getMethod == null)
                throw new ArgumentNullException(nameof(getMethod));

            DynamicMethod dm = new DynamicMethod("PropertyGetter", typeof(TValue), new Type[] { typeof(TDeclaring) }, typeof(TDeclaring), true);

            ILGenerator il = dm.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.EmitCall(OpCodes.Callvirt, getMethod, null);
            il.Emit(OpCodes.Ret);

            return (Func<TDeclaring, TValue>) dm.CreateDelegate(typeof(Func<TDeclaring, TValue>));
        }

        public static Action<TDeclaring, TValue> CreatePropertySetter<TDeclaring, TValue>(MethodInfo setMethod)
        {
            if (setMethod == null)
                throw new ArgumentNullException(nameof(setMethod));

            DynamicMethod dm = new DynamicMethod("PropertySetter", null, new Type[] { typeof(TDeclaring), typeof(TValue) }, typeof(TDeclaring), true);

            ILGenerator il = dm.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, setMethod, null);
            il.Emit(OpCodes.Ret);

            return (Action<TDeclaring, TValue>) dm.CreateDelegate(typeof(Action<TDeclaring, TValue>));
        }
    }
}