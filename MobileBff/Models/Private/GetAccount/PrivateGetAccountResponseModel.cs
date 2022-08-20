using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccount;

namespace MobileBff.Models.Private.GetAccount
{
    public class PrivateGetAccountResponseModel
    {
        public PrivateGetAccountResponseModel(GetAccountResult result)
        {
            RetrievedDateTime = result.RetrievedDateTime;

            ResourceId = result.Account.Identifications.ResourceId;

            Details = new[]
            {
                new PrivateDetailModel(result.Account)
            };
        }

        [JsonPropertyName("retrieved_date_time")]
        public DateTime RetrievedDateTime { get; }

        [JsonPropertyName("resource_id")]
        public string ResourceId { get; }

        [JsonPropertyName("details")]
        public PrivateDetailModel[] Details { get; }
    }
}