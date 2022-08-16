using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccount;

namespace MobileBff.Models.Shared.GetAccount
{
    public class AccountResponseModel
    {
        public AccountResponseModel(GetAccountResult result)
        {
            RetrievedDateTime = result.RetrievedDateTime;

            ResourceId = result.Account.Identifications.ResourceId;

            Details = new[]
            {
                new DetailModel(result.Account)
            };
        }

        [JsonPropertyName("retrieved_date_time")]
        public DateTime RetrievedDateTime { get; }

        [JsonPropertyName("resource_id")]
        public string ResourceId { get; }

        [JsonPropertyName("details")]
        public DetailModel[] Details { get; }
    }
}