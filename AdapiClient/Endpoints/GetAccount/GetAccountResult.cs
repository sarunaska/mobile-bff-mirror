#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using AdapiClient.Models;

namespace AdapiClient.Endpoints.GetAccount
{
    public class GetAccountResult
    {
        public DateTime RetrievedDateTime { get; set; }
        public Account Account { get; set; }
    }
}
