using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Attributes;
using MobileBff.ExtensionMethods;

namespace MobileBff.Models.Shared.GetAccountReservedAmounts
{
    public class Reservation
    {
        [BffRequired]
        [JsonPropertyName("origin")]
        public string? Origin { get; }

        [BffRequired]
        [JsonPropertyName("reservation_date")]
        public string? ReservationDate { get; }

        [BffRequired]
        [JsonPropertyName("amount")]
        public decimal? Amount { get; }

        [BffRequired]
        [JsonPropertyName("currency")]
        public string? Currency { get; }

        [BffRequired]
        [JsonPropertyName("message")]
        public string? Message { get; }

        public Reservation(AccountReservation accountReservation)
        {
            Origin = accountReservation.Origin;
            ReservationDate = accountReservation.ReservationDate;
            Amount = accountReservation.Amount?.ToBankingDecimal();
            Currency = accountReservation.Currency;
            Message = accountReservation.DescriptiveText;
        }
    }
}
