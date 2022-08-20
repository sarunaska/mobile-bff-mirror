using System.Text.Json.Serialization;

namespace MobileBff.Models.Shared.GetAccount
{
    public class ItemModel
    {
        private const string ItemTypeKeyValue = "KeyValue";
        private const string ItemTypeActionableKeyValue = "ActionableKeyValue";
        private const string ItemTypeKeyValueList = "KeyValueList";

        [JsonPropertyName("type")]
        public string Type { get; }

        [JsonPropertyName("title")]
        public string Title { get; }

        [JsonPropertyName("value")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Value { get; }

        [JsonPropertyName("value_list")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? ValueList { get; }

        [JsonPropertyName("action")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Action { get; }

        public ItemModel(string title, string value, string action)
        {
            Type = ItemTypeActionableKeyValue;
            Title = title;
            Value = value;
            Action = action;
        }

        public ItemModel(string title, string value)
        {
            Type = ItemTypeKeyValue;
            Title = title;
            Value = value;
        }

        public ItemModel(string title, string[] valueList)
        {
            Type = ItemTypeKeyValueList;
            Title = title;
            ValueList = valueList;
        }
    }
}