using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccountReservedAmounts;
using MobileBff.Models.Shared.GetAccountReservedAmounts;

namespace MobileBff.Models.Youth.GetAccountReservedAmounts
{
    public class YouthGetAccountReservedAmountsResponseModel : AccountReservedAmountsResponseModel, IPartialResponseModel
    {
        [JsonPropertyName("reservations")]
        public List<Reservation>? Reservations { get; }

        public YouthGetAccountReservedAmountsResponseModel(GetAccountReservedAmountsResult? result) : base(result)
        {
            if (result?.AccountReservations != null)
            {
                Reservations = result.AccountReservations.Select(x => new Reservation(x)).ToList();
            }
        }
    }
}