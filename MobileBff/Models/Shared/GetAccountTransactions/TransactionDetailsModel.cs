using System.Text.Json.Serialization;
using AdapiClient.Models;

namespace MobileBff.Models.Shared.GetAccountTransactions
{
    public class TransactionDetailsModel
    {
        [JsonPropertyName("href")]
        public string? Href { get; }

        public TransactionDetailsModel(TransactionDetails transactionDetails)
        {
            Href = transactionDetails.Href;
        }
    }
}
