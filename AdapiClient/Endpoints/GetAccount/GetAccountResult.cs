using AdapiClient.Models;

namespace AdapiClient.Endpoints.GetAccount
{
    public class GetAccountResult
    {
        public DateTime? RetrievedDateTime { get; set; }
        public Account? Account { get; set; }
    }
}
