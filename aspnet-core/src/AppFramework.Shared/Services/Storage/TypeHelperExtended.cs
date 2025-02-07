using System;
using System.Reflection;

namespace AppFramework.Shared.Services
{
    public class TypeHelperExtended
    {
        public static bool IsPrimitiveIncludingNullable(Type type, bool includeEnums = false)
        {
            if (IsPrimitive(type, includeEnums))
            {
                return true;
            }

            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return IsPrimitive(type.GenericTypeArguments[0], includeEnums);
            }

            return false;
        }

        public static bool IsPrimitive(Type type, bool includeEnums)
        {
            if (type.GetTypeInfo().IsPrimitive)
            {
                return true;
            }

            if (includeEnums && type.GetTypeInfo().IsEnum)
            {
                return true;
            }

            return type == typeof(string) ||
                   type == typeof(decimal) ||
                   type == typeof(DateTime) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(Guid);
        }
    }
}