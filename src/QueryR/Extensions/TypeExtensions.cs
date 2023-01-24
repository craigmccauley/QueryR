using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QueryR.Extensions
{
    internal static class TypeExtensions
    {
        //TODO: having issues with Queryable.Select 2 different methods
        public static MethodInfo GetGenericMethod(this Type type, string methodName, int genericArgumentCount, int parameterCount, params Type[] typeArguments)
        {
            return type.GetMethods().First(
                    method => method.Name == methodName
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == genericArgumentCount
                        && method.GetParameters().Length == parameterCount)
                    .MakeGenericMethod(typeArguments);
        }


        /// <summary>
        ///     Finds the type of the element of a type. Returns null if this type does not enumerate.
        ///     From https://stackoverflow.com/a/55244482/102526
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>The element type, if found; otherwise, <see langword="null"/>.</returns>
        public static Type FindElementType(this Type type)
        {
            if (type.IsArray)
                return type.GetElementType();

            // type is IEnumerable<T>;
            if (ImplIEnumT(type))
                return type.GetGenericArguments().First();

            // type implements/extends IEnumerable<T>;
            var enumType = type.GetInterfaces().Where(ImplIEnumT).Select(t => t.GetGenericArguments().First()).FirstOrDefault();
            if (enumType != null)
                return enumType;

            // type is IEnumerable
            if (IsIEnum(type) || type.GetInterfaces().Any(IsIEnum))
                return typeof(object);

            return null;

            bool IsIEnum(Type t) => t == typeof(System.Collections.IEnumerable);
            bool ImplIEnumT(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }
    }
}
