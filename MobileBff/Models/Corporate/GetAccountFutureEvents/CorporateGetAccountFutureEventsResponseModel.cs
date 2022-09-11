using AdapiClient.Endpoints.GetAccountFutureEvents;
using MobileBff.Models.Shared.GetAccountTransactions;

namespace MobileBff.Models.Corporate.GetAccountFutureEvents
{
    public class CorporateGetAccountFutureEventsResponseModel : AccountFutureEventsResponseModel, IPartialResponseModel
    {
        public CorporateGetAccountFutureEventsResponseModel(GetAccountFutureEventsResult? result) : base(result)
        {
        }
    }
}