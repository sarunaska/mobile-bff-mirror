using AdapiClient.Endpoints.GetAccount;
using MobileBff.Models.Shared.GetAccount;

namespace MobileBff.Models.Youth.GetAccount
{
    public class YouthGetAccountResponseModel : AccountResponseModel
    {
        public YouthGetAccountResponseModel(GetAccountResult result) : base(result)
        {
        }
    }
}