using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccountTransactions;

namespace MobileBff.Models.Shared.GetAccountTransactions
{
    public class AccountTransactionsResponseModel
    {
        [JsonPropertyName("retrieved_date_time")]
        public DateTime RetrievedDateTime { get; }

        [JsonPropertyName("account")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TransactionsAccountModel Account { get; set; }

        [JsonPropertyName("paginating_information")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PaginatingInformationModel PaginatingInformation { get; set; }

        public AccountTransactionsResponseModel(GetAccountTransactionsResult? result)
        {
            RetrievedDateTime = result?.RetrievedDateTime ?? DateTime.UtcNow;

            Account = new TransactionsAccountModel(result?.Account?.Identifications);

            PaginatingInformation = new PaginatingInformationModel(result?.PaginatingInformation);
        }
    }
}