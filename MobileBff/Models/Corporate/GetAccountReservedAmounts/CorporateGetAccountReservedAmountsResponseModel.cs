using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccountReservedAmounts;
using MobileBff.Models.Shared.GetAccountReservedAmounts;

namespace MobileBff.Models.Corporate.GetAccountReservedAmounts
{
    public class CorporateGetAccountReservedAmountsResponseModel : AccountReservedAmountsResponseModel, IPartialResponseModel
    {
        [JsonPropertyName("reservations")]
        public List<Reservation>? Reservations { get; }

        public CorporateGetAccountReservedAmountsResponseModel(GetAccountReservedAmountsResult? result) : base(result)
        {
            if (result?.AccountReservations != null)
            {
                Reservations = result.AccountReservations.Select(x => new Reservation(x)).ToList();
            }
        }
    }
}