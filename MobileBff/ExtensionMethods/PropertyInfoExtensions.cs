using System.Collections;
using System.Reflection;
using MobileBff.Attributes;

namespace MobileBff.ExtensionMethods
{
    public static class PropertyInfoExtensions
    {
        private const string SystemNamespaceName = "System";

        public static bool IsIndexer(this PropertyInfo property)
        {
            return property.GetIndexParameters().Any();
        }

        public static bool IsRequired(this PropertyInfo property)
        {
            return property.CustomAttributes.Any(x => x.AttributeType == typeof(BffRequiredAttribute));
        }

        public static bool IsList(this PropertyInfo property)
        {
            return property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>);
        }

        public static bool IsArray(this PropertyInfo property)
        {
            return property.PropertyType.IsArray;
        }

        public static bool IsObject(this PropertyInfo property)
        {
            return property.PropertyType.IsClass && !IsSystem(property);
        }

        public static bool IsSystem(this PropertyInfo property)
        {
            return property.PropertyType.Namespace != null && property.PropertyType.Namespace.StartsWith(SystemNamespaceName);
        }
    }
}
