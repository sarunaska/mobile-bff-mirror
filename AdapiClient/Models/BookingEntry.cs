using System.Text.Json.Serialization;

namespace AdapiClient.Models
{
    public class BookingEntry
    {
        public string? TransactionId { get; set; }
        public string? ExternalId { get; set; }
        public string? ValueDate { get; set; }
        public string? BookingDate { get; set; }
        public string? Message1 { get; set; }
        public BankTransactionCode? BankTransactionCode { get; set; }
        public TransactionAmount? TransactionAmount { get; set; }
        public BookedBalance? BookedBalance { get; set; }
        public CardBookingEntryDetails? CardBookingEntryDetails { get; set; }

        [JsonPropertyName("_links")]
        public Links? Links { get; set; }
    }
}
