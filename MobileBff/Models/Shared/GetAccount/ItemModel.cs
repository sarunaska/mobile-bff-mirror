using System.Text.Json.Serialization;
using MobileBff.Attributes;

namespace MobileBff.Models.Shared.GetAccount
{
    public class ItemModel : ICustomPartialResponse
    {
        private const string ItemTypeKeyValue = "KeyValue";
        private const string ItemTypeActionableKeyValue = "ActionableKeyValue";
        private const string ItemTypeKeyValueList = "KeyValueList";

        [BffRequired]
        [JsonPropertyName("type")]
        public string Type { get; }

        [BffRequired]
        [JsonPropertyName("title")]
        public string Title { get; }

        [JsonPropertyName("value")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Value { get; }

        [JsonPropertyName("value_list")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? ValueList { get; }

        [JsonPropertyName("action")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Action { get; }

        [JsonIgnore]
        public bool IsPartialResponse
        {
            get
            {
                return Value == null && (ValueList == null || ValueList.Any());
            }
        }

        public ItemModel(string title, string? value, string action)
        {
            Type = ItemTypeActionableKeyValue;
            Title = title;
            Value = value;
            Action = action;
        }

        public ItemModel(string title, string? value)
        {
            Type = ItemTypeKeyValue;
            Title = title;
            Value = value;
        }

        public ItemModel(string title, string?[] valueList)
        {
            Type = ItemTypeKeyValueList;
            Title = title;
            ValueList = valueList.Where(x => x != null).Select(x => x!).ToList();
        }
    }
}