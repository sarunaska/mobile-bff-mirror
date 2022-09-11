using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Attributes;

namespace MobileBff.Models.Shared.GetAccountTransactions
{
    public class TransactionsAccountModel
    {
        [BffRequired]
        [JsonPropertyName("resource_id")]
        public string? ResourceId { get; }

        [BffRequired]
        [JsonPropertyName("account_number")]
        public string? AccountNumber { get; }

        public TransactionsAccountModel(Identifications? identifications)
        {
            ResourceId = identifications?.ResourceId;
            AccountNumber = identifications?.DomesticAccountNumber;
        }
    }
}
