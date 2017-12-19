using System;
using System.Reflection;

namespace FastXmlSerialization
{
    internal class PropertyAccessor<TDeclaring, TValue> : IMemberAccessor<TDeclaring, TValue>
    {
        private readonly PropertyInfo _propertyInfo;

        private readonly Func<TDeclaring, TValue> _getter;
        private readonly Action<TDeclaring, TValue> _setter;
        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            this._propertyInfo = propertyInfo;

            //默认为属性是可读可写的。
            MethodInfo getMethod = this._propertyInfo.GetGetMethod(true);
            MethodInfo setMethod = this._propertyInfo.GetSetMethod(true);

            this._getter = TypeExtensions.CreatePropertyGetter<TDeclaring, TValue>(getMethod);
            this._setter = TypeExtensions.CreatePropertySetter<TDeclaring, TValue>(setMethod);
        }

        //todo:暂时直接反射，后期优化。
        object IMemberAccessor.GetValue(object obj)
        {
            //return this._propertyInfo.GetValue(obj);
            return this.GetValue((TDeclaring) obj);
        }

          void IMemberAccessor<TValue>.SetValue(object obj, TValue value)
        {
              this.SetValue((TDeclaring)obj, value);
        }


         TValue IMemberAccessor<TValue>.GetValue(object obj)
         {
             return this.GetValue((TDeclaring) obj);
         }

        void IMemberAccessor.SetValue(object obj, object value)
        {
            //this._propertyInfo.SetValue(obj, value);
            this.SetValue((TDeclaring)obj, (TValue)value);
        }

      
        public TValue GetValue(TDeclaring obj)
        {
            return this._getter.Invoke(obj);
        }

        public void SetValue(TDeclaring obj, TValue value)
        {
            this._setter.Invoke(obj, value);
        }
    }
}