using System;

namespace JsonQueryable.Extensions
{
    internal static class TypeExtensions
    {
        public static Type FindBaseType(this Type type, Func<Type, bool> baseTypeFilter)
        {
            Type currentBaseType = type.BaseType;

            if (currentBaseType == null)
            {
                return null;
            }
            else if (baseTypeFilter(currentBaseType))
            {
                return currentBaseType;
            }

            return currentBaseType.FindBaseType(baseTypeFilter);
        }
    }
}
