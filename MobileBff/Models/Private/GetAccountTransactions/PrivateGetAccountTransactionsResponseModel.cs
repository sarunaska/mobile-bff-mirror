using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccountTransactions;
using MobileBff.Attributes;
using MobileBff.Models.Shared.GetAccountTransactions;

namespace MobileBff.Models.Private.GetAccountTransactions
{
    public class PrivateGetAccountTransactionsResponseModel : AccountTransactionsResponseModel, IPartialResponseModel
    {
        [BffRequired]
        [JsonPropertyName("entries")]
        public List<PrivateEntryModel>? Entries { get; }

        public PrivateGetAccountTransactionsResponseModel(GetAccountTransactionsResult? result) : base(result)
        {
            Entries = result?.DepositEntryEvent?.BookingEntries?.Select(x => new PrivateEntryModel(x)).ToList();
        }
    }
}