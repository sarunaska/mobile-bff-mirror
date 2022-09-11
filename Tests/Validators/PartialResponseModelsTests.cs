using System.Reflection;
using System.Text.Json.Serialization;
using MobileBff.ExtensionMethods;
using MobileBff.Models;

namespace Tests.Validators
{
    public class PartialResponseModelsTests
    {
        [Fact]
        public void AllResponseModelsHaveSettersInTopLevelObjectProperties()
        {
            var models = GetResponseModels();

            var invalidProperties = models.SelectMany(GetTopLevelObjectPropertiesWithoutSetters).ToList();

            Assert.Empty(invalidProperties);
        }

        [Fact]
        public void AllResponseModelsHaveJsonIgnoreWhenWritingNullAttributeInTopLevelObjectProperties()
        {
            var models = GetResponseModels();

            var invalidProperties = models.SelectMany(GetTopLevelObjectPropertiesWithoutJsonIgnoreWhenWritingNullAttributes).ToList();

            Assert.Empty(invalidProperties);
        }

        [Fact]
        public void NoResponseModelsHaveArrayProperties()
        {
            var models = GetResponseModels();

            var invalidProperties = models.SelectMany(GetPropertiesWithArrays).ToList();

            Assert.Empty(invalidProperties);
        }

        private static List<Type> GetResponseModels()
        {
            var interfaceType = typeof(IPartialResponseModel);
            var classTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(x => interfaceType.IsAssignableFrom(x) && x != interfaceType)
                .ToList();

            return classTypes;
        }

        private static IEnumerable<PropertyInfo> GetTopLevelObjectPropertiesWithoutSetters(Type type)
        {
            return type.GetProperties().Where(x => x.IsObject() && x.SetMethod == null);
        }

        private static List<PropertyInfo> GetTopLevelObjectPropertiesWithoutJsonIgnoreWhenWritingNullAttributes(Type type)
        {
            return type.GetProperties()
                .Where(prop => prop.IsObject() && prop.GetCustomAttributes().All(attr => (attr as JsonIgnoreAttribute)?.Condition != JsonIgnoreCondition.WhenWritingNull))
                .ToList();
        }

        private static List<PropertyInfo> GetPropertiesWithArrays(Type type)
        {
            var allArrayProperties = new List<PropertyInfo>();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (property.IsArray())
                {
                    allArrayProperties.Add(property);
                    continue;
                }

                if (property.IsList())
                {
                    var listItemType = property.PropertyType.GetGenericArguments()[0];
                    var arrayProperties = GetPropertiesWithArrays(listItemType);
                    allArrayProperties.AddRange(arrayProperties);
                }

                if (property.IsObject())
                {
                    var arrayProperties = GetPropertiesWithArrays(property.PropertyType);
                    allArrayProperties.AddRange(arrayProperties);
                }
            }

            return allArrayProperties;
        }
    }
}
