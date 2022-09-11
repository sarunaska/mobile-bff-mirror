using AdapiClient.Endpoints.GetAccountFutureEvents;
using MobileBff.Models.Shared.GetAccountTransactions;

namespace MobileBff.Models.Private.GetAccountFutureEvents
{
    public class PrivateGetAccountFutureEventsResponseModel : AccountFutureEventsResponseModel, IPartialResponseModel
    {
        public PrivateGetAccountFutureEventsResponseModel(GetAccountFutureEventsResult? result) : base(result)
        {
        }
    }
}