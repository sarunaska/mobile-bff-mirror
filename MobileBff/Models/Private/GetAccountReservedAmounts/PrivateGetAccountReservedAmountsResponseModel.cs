using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccountReservedAmounts;
using MobileBff.Models.Shared.GetAccountReservedAmounts;

namespace MobileBff.Models.Private.GetAccountReservedAmounts
{
    public class PrivateGetAccountReservedAmountsResponseModel : AccountReservedAmountsResponseModel, IPartialResponseModel
    {
        [JsonPropertyName("reservations")]
        public List<PrivateReservation>? Reservations { get; }

        public PrivateGetAccountReservedAmountsResponseModel(GetAccountReservedAmountsResult? result) : base(result)
        {
            if (result?.AccountReservations != null)
            {
                Reservations = result.AccountReservations.Select(x => new PrivateReservation(x)).ToList();
            }
        }
    }
}