using AdapiClient.Models;

namespace AdapiClient.Endpoints.GetAccountReservedAmounts
{
    public class GetAccountReservedAmountsResult
    {
        public DateTime? RetrievedDateTime { get; set; }
        public Account? Account { get; set; }
        public AccountReservation[]? AccountReservations { get; set; }
}
}
