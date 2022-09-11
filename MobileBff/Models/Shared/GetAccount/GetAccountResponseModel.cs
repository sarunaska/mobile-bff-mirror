using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccount;
using MobileBff.Attributes;

namespace MobileBff.Models.Shared.GetAccount
{
    public class GetAccountResponseModel
    {
        [JsonPropertyName("retrieved_date_time")]
        public DateTime RetrievedDateTime { get; }

        [BffRequired]
        [JsonPropertyName("resource_id")]
        public string? ResourceId { get; }

        public GetAccountResponseModel(GetAccountResult? result)
        {
            RetrievedDateTime = result?.RetrievedDateTime ?? DateTime.UtcNow;

            ResourceId = result?.Account?.Identifications?.ResourceId;
        }
    }
}
