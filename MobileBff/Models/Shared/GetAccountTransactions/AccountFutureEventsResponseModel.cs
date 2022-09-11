using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccountFutureEvents;
using MobileBff.Models.Shared.GetAccountFutureEvents;

namespace MobileBff.Models.Shared.GetAccountTransactions
{
    public class AccountFutureEventsResponseModel
    {
        [JsonPropertyName("retrieved_date_time")]
        public DateTime RetrievedDateTime { get; }

        [JsonPropertyName("account")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TransactionsAccountModel Account { get; set; }

        [JsonPropertyName("future_events")]
        public List<FutureEventModel>? FutureEvents { get; }

        public AccountFutureEventsResponseModel(GetAccountFutureEventsResult? result)
        {
            RetrievedDateTime = result?.RetrievedDateTime ?? DateTime.UtcNow;

            Account = new TransactionsAccountModel(result?.Account?.Identifications);

            FutureEvents = result?.FutureEvents?.Select(x => new FutureEventModel(x)).ToList();
        }
    }
}