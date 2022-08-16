using AdapiClient.Endpoints.GetAccount;
using MobileBff.Models.Shared.GetAccount;

namespace MobileBff.Models.Corporate.GetAccount
{
    public class CorporateGetAccountResponseModel : AccountResponseModel
    {
        public CorporateGetAccountResponseModel(GetAccountResult result) : base(result)
        {
        }
    }
}