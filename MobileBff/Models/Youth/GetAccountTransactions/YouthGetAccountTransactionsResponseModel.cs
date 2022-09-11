using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccountTransactions;
using MobileBff.Attributes;
using MobileBff.Models.Shared.GetAccountTransactions;

namespace MobileBff.Models.Youth.GetAccountTransactions
{
    public class YouthGetAccountTransactionsResponseModel : AccountTransactionsResponseModel, IPartialResponseModel
    {
        [BffRequired]
        [JsonPropertyName("entries")]
        public List<EntryModel>? Entries { get; }

        public YouthGetAccountTransactionsResponseModel(GetAccountTransactionsResult? result) : base(result)
        {
            Entries = result?.DepositEntryEvent?.BookingEntries?.Select(x => new EntryModel(x)).ToList();
        }
    }
}