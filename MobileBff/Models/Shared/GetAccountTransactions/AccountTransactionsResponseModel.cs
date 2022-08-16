using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccountTransactions;

namespace MobileBff.Models.Shared.GetAccountTransactions
{
    public class AccountTransactionsResponseModel
    {
        public AccountTransactionsResponseModel(GetAccountTransactionsResult result)
        {
            RetrievedDateTime = result.RetrievedDateTime;

            Account = new TransactionsAccountModel(result.Account.Identifications);

            PaginatingInformation = new PaginatingInformationModel(result.PaginatingInformation);

            Entries = result.DepositEntryEvent.BookingEntries.Select(x => new EntryModel(x)).ToArray();
        }

        [JsonPropertyName("retrieved_date_time")]
        public DateTime RetrievedDateTime { get; }

        [JsonPropertyName("account")]
        public TransactionsAccountModel Account { get; }

        [JsonPropertyName("paginating_information")]
        public PaginatingInformationModel PaginatingInformation { get; }

        [JsonPropertyName("entries")]
        public EntryModel[] Entries { get; }
    }
}