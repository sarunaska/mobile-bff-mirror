using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccounts;

namespace MobileBff.Models.Shared.GetAccounts
{
    public class GetAccountsResponseModel
    {
        [JsonPropertyName("retrieved_date_time")]
        public DateTime RetrievedDateTime { get; }

        public GetAccountsResponseModel(GetAccountsResult? result)
        {
            RetrievedDateTime = result?.RetrievedDateTime ?? DateTime.UtcNow;
        }
    }
}
