using AdapiClient.Endpoints.GetAccountTransactions;
using MobileBff.Models.Shared.GetAccountTransactions;

namespace MobileBff.Models.Corporate.GetAccountTransactions
{
    public class CorporateGetAccountTransactionsResponseModel : AccountTransactionsResponseModel
    {
        public CorporateGetAccountTransactionsResponseModel(GetAccountTransactionsResult result) : base(result)
        {
        }
    }
}