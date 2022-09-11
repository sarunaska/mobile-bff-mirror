using System.Text.Json.Serialization;
using SebCsClient.Models;

namespace MobileBff.Models.Shared.GetAccounts
{
    public class OwnerModel
    {
        [JsonPropertyName("id")]
        public string? Id { get; }

        [JsonPropertyName("name")]
        public string? Name { get; }

        public OwnerModel(AccountOwner accountOwner)
        {
            Id = accountOwner.Id;
            Name = accountOwner.Name;
        }
    }
}
