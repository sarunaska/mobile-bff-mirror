using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccountReservedAmounts;

namespace MobileBff.Models.Shared.GetAccountReservedAmounts
{
    public class AccountReservedAmountsResponseModel
    {
        [JsonPropertyName("retrieved_date_time")]
        public DateTime RetrievedDateTime { get; }

        [JsonPropertyName("account")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ReservedAmountsAccountModel Account { get; set; }

        public AccountReservedAmountsResponseModel(GetAccountReservedAmountsResult? result)
        {
            RetrievedDateTime = result?.RetrievedDateTime ?? DateTime.UtcNow;

            Account = new ReservedAmountsAccountModel(result?.Account?.Identifications);
        }
    }
}
