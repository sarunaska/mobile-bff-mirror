using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Attributes;
using MobileBff.Models.Shared.GetAccountReservedAmounts;

namespace MobileBff.Models.Private.GetAccountReservedAmounts
{
    public class PrivateReservation : Reservation
    {
        [BffRequired]
        [JsonPropertyName("external_id")]
        public string? ExternalId { get; }

        public PrivateReservation(AccountReservation accountReservation) : base(accountReservation)
        {
            ExternalId = accountReservation.ControlId;
        }
    }
}
