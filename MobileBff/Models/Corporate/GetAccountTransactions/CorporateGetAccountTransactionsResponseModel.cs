using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccountTransactions;
using MobileBff.Attributes;
using MobileBff.Models.Shared.GetAccountTransactions;

namespace MobileBff.Models.Corporate.GetAccountTransactions
{
    public class CorporateGetAccountTransactionsResponseModel : AccountTransactionsResponseModel, IPartialResponseModel
    {
        [BffRequired]
        [JsonPropertyName("entries")]
        public List<EntryModel>? Entries { get; }

        public CorporateGetAccountTransactionsResponseModel(GetAccountTransactionsResult? result) : base(result)
        {
            Entries = result?.DepositEntryEvent?.BookingEntries?.Select(x => new EntryModel(x)).ToList();
        }
    }
}