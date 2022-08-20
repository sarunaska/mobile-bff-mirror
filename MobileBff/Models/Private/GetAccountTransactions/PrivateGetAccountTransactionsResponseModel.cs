using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccountTransactions;
using MobileBff.Models.Shared.GetAccountTransactions;

namespace MobileBff.Models.Private.GetAccountTransactions
{
    public class PrivateGetAccountTransactionsResponseModel
    {
        public PrivateGetAccountTransactionsResponseModel(GetAccountTransactionsResult result)
        {
            RetrievedDateTime = result.RetrievedDateTime;

            Account = new TransactionsAccountModel(result.Account.Identifications);

            PaginatingInformation = new PaginatingInformationModel(result.PaginatingInformation);

            Entries = result.DepositEntryEvent.BookingEntries.Select(x => new PrivateEntryModel(x)).ToArray();
        }

        [JsonPropertyName("retrieved_date_time")]
        public DateTime RetrievedDateTime { get; }

        [JsonPropertyName("account")]
        public TransactionsAccountModel Account { get; }

        [JsonPropertyName("paginating_information")]
        public PaginatingInformationModel PaginatingInformation { get; }

        [JsonPropertyName("entries")]
        public PrivateEntryModel[] Entries { get; }
    }
}