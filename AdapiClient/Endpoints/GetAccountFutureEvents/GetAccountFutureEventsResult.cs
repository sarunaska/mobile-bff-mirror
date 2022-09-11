using AdapiClient.Models;

namespace AdapiClient.Endpoints.GetAccountFutureEvents
{
    public class GetAccountFutureEventsResult
    {
        public DateTime? RetrievedDateTime { get; set; }
        public Account? Account { get; set; }
        public FutureEvent[]? FutureEvents { get; set; }
    }
}
