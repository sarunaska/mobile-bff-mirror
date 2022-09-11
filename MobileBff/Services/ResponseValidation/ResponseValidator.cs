using System.Collections;
using System.Reflection;
using MobileBff.ExtensionMethods;
using MobileBff.Models.Shared;

namespace MobileBff.Services.ResponseValidation
{
    public class ResponseValidator : IResponseValidator
    {
        private const string PropertyIsPartialResponse = "IsPartialResponse";
        private bool isValid = true;

        public bool ValidateAndUpdate<T>(T model)
        {
            try
            {
                ValidateObject(model, true);
            }
            catch (ResponseValidationException ex)
            {
                Log(ex);
                return false;
            }

            return isValid;
        }

        private static bool IsPropertyNullOrEmpty(object? value, PropertyInfo property)
        {
            if (value == null)
            {
                return true;
            }

            if (value is string && string.IsNullOrEmpty(value as string))
            {
                return true;
            }

            if (property.IsList() && (value as IList)?.Count == 0)
            {
                return true;
            }

            return false;
        }

        private static void Log(string message)
        {
            Console.Error.WriteLine(message);
        }

        private static void Log(ResponseValidationException exception)
        {
            Console.Error.WriteLine(exception.Message);
        }

        private void ValidateObject<T>(T model, bool setNotValidPropertiesToNull = false)
        {
            if (model == null)
            {
                return;
            }

            foreach (var property in model.GetType().GetProperties())
            {
                try
                {
                    ValidateProperty(model, property);
                }
                catch (ResponseValidationException)
                {
                    if (setNotValidPropertiesToNull && property.SetMethod != null)
                    {
                        property.SetValue(model, null);
                    }

                    throw;
                }
            }

            ValidateObjectCustomPartialResponseLogic(model);
        }

        private void ValidateObjectCustomPartialResponseLogic<T>(T model)
        {
            if (model?.GetType().GetInterface(nameof(ICustomPartialResponse)) == null)
            {
                return;
            }

            var isPartialResponseProperty = model.GetType().GetProperty(PropertyIsPartialResponse);

            var isPartialResponse = isPartialResponseProperty!.GetValue(model, null) as bool?;
            if (isPartialResponse == true)
            {
                throw new ResponseValidationException(model.GetType());
            }
        }

        private void ValidateProperty<T>(T model, PropertyInfo property)
        {
            if (property.IsIndexer())
            {
                return;
            }

            var isRequired = property.IsRequired();
            var value = property.GetValue(model, null);

            if (IsPropertyNullOrEmpty(value, property))
            {
                if (isRequired)
                {
                    throw new ResponseValidationException(property);
                }

                return;
            }

            if (property.IsList() && value is IList)
            {
                ValidateListItems((IList)value);
            }

            if (property.IsObject())
            {
                ValidateObject(value);
            }

            return;
        }

        private void ValidateListItems(IList list)
        {
            var itemsToRemove = new List<object>();
            foreach (var item in list)
            {
                try
                {
                    ValidateObject(item);
                }
                catch (ResponseValidationException ex)
                {
                    Log(ex);

                    if (!list.IsFixedSize)
                    {
                        itemsToRemove.Add(item);
                    }
                }
            }

            itemsToRemove.ForEach(x => list.Remove(x));

            if (itemsToRemove.Any())
            {
                isValid = false;
            }
        }
    }
}
