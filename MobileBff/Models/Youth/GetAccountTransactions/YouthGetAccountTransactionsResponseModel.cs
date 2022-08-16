using AdapiClient.Endpoints.GetAccountTransactions;
using MobileBff.Models.Shared.GetAccountTransactions;

namespace MobileBff.Models.Youth.GetAccountTransactions
{
    public class YouthGetAccountTransactionsResponseModel : AccountTransactionsResponseModel
    {
        public YouthGetAccountTransactionsResponseModel(GetAccountTransactionsResult result) : base(result)
        {
        }
    }
}