using System;
using System.Reflection;
using GitTask.Domain.Exception;

namespace GitTask.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class KeyAttribute : Attribute
    {
        public string PropertyName { get; }
        public KeyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public static PropertyInfo GetKeyProperty(Type type)
        {
            var attribute = GetCustomAttribute(type, typeof(KeyAttribute));
            if (!(attribute is KeyAttribute))
            {
                throw new KeyAttributeNotDefinedException(type);
            }
            var keyProperty = type.GetProperty(((KeyAttribute)attribute).PropertyName);
            if (keyProperty == null)
            {
                throw new KeyPropertyNotExistsException(type);
            }
            return keyProperty;
        }

        public static object GetKeyValue<TType>(TType objectWithKey)
        {
            return GetKeyProperty(typeof(TType)).GetValue(objectWithKey);
        }
    }
}
