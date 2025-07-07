using System;
using System.Reflection;

namespace CoreEssentials.Utils
{
    internal static class ReflectionUtils
    {
        internal static T? CreateInstance<T>(Type type, params object[] args) where T : class
        {
            return Activator.CreateInstance(type, args) as T;
        }

        internal static void SetInstanceField(object instance, Type type, string fieldName, object value)
        {
            var field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            field?.SetValue(instance, value);
        }
    }
}
