using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Attributes;

namespace MobileBff.Models.Shared.GetAccountReservedAmounts
{
    public class ReservedAmountsAccountModel
    {
        [BffRequired]
        [JsonPropertyName("resource_id")]
        public string? ResourceId { get; }

        [BffRequired]
        [JsonPropertyName("account_number")]
        public string? AccountNumber { get; }

        public ReservedAmountsAccountModel(Identifications? identifications)
        {
            ResourceId = identifications?.ResourceId;
            AccountNumber = identifications?.DomesticAccountNumber;
        }
    }
}
