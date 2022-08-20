using System.Text.Json.Serialization;

namespace MobileBff.Models.Shared.GetAccounts
{
    public class OwnerModel
    {
        [JsonPropertyName("id")]
        public string Id { get; }

        [JsonPropertyName("name")]
        public string Name { get; }

        public OwnerModel(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
