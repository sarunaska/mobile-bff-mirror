using System.Text.Json.Serialization;
using AdapiClient.Models;

namespace MobileBff.Models.Shared.GetAccountTransactions
{
    public class TransactionsAccountModel
    {
        [JsonPropertyName("resource_id")]
        public string ResourceId { get; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; }

        public TransactionsAccountModel(Identifications identifications)
        {
            ResourceId = identifications.ResourceId;
            AccountNumber = identifications.DomesticAccountNumber;
        }
    }
}
