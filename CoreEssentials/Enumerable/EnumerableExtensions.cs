using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreEssentials.Enumerable
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Gets the type of elements in an IEnumerable&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to get the element type from.</param>
        /// <returns>The Type of elements in the enumerable.</returns>
        public static Type GetElementType<T>(this IEnumerable<T> enumerable)
        {
            return typeof(T);
        }

        /// <summary>
        /// Gets the element type of an object if it implements IEnumerable&lt;T&gt;.
        /// </summary>
        /// <param name="obj">The object to get the element type from.</param>
        /// <returns>The element type if the object is an IEnumerable&lt;T&gt;; otherwise, null.</returns>
        public static Type? GetElementType(this object obj)
        {
            if (obj == null)
                return null;

            Type type = obj.GetType();
            
            // If this is already an array type, return its element type
            if (type.IsArray)
                return type.GetElementType();

            // If type itself is IEnumerable<T>
            if (type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return type.GetGenericArguments()[0];
            }

            // Look for IEnumerable<T> interfaces
            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType && 
                    interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    // Return the first generic argument (T in IEnumerable<T>)
                    return interfaceType.GetGenericArguments()[0];
                }
            }

            // If it's IEnumerable but not generic (like ArrayList), return object type
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                return typeof(object);
            }

            return null;
        }
    }
}
