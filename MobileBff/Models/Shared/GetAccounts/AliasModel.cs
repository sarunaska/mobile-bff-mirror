using System.Text.Json.Serialization;
using AdapiClient.Models;

namespace MobileBff.Models.Shared.GetAccounts
{
    public class AliasModel
    {
        [JsonPropertyName("type")]
        public string? Type { get; }

        [JsonPropertyName("id")]
        public string? Id { get; }

        public AliasModel(Alias alias)
        {
            Id = alias.Id;
            Type = alias.Type;
        }
    }
}