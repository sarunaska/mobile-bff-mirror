using System.Reflection;

namespace MobileBff.Services.ResponseValidation
{
    public class ResponseValidationException : Exception
    {
        public ResponseValidationException(PropertyInfo property)
            : base($"Required property {property.Name} is not set in response model {property.ReflectedType?.FullName}.")
        {
        }

        public ResponseValidationException(Type type)
            : base($"Not all required properties are set for object {type} in response model.")
        {
        }
    }
}
