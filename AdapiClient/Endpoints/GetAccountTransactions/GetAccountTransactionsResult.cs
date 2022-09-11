using AdapiClient.Models;

namespace AdapiClient.Endpoints.GetAccountTransactions
{
    public class GetAccountTransactionsResult
    {
        public DateTime? RetrievedDateTime { get; set; }
        public Account? Account { get; set; }
        public PaginatingInformation? PaginatingInformation { get; set; }
        public DepositEntryEvent? DepositEntryEvent { get; set; }
    }
}
