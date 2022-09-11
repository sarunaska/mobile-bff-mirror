using AdapiClient.Models;

namespace AdapiClient.Endpoints.GetAccounts
{
    public class GetAccountsResult
    {
        public DateTime? RetrievedDateTime { get; set; }
        public Account[]? Accounts { get; set; }
    }
}
