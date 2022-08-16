using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Models.Shared.GetAccountTransactions;

namespace MobileBff.Models.Private.GetAccountTransactions
{
    public class PrivateEntryModel : EntryModel
    {
        [JsonPropertyName("external_id")]
        public string ExternalId { get; }

        public PrivateEntryModel(BookingEntry bookingEntry) : base(bookingEntry)
        {
            ExternalId = bookingEntry.ExternalId;
        }
    }
}
