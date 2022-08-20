using MobileBff.Models.Private.GetAccount;
using MobileBff.Models.Private.GetAccounts;
using MobileBff.Models.Private.GetAccountTransactions;

namespace MobileBff.Services
{
    public interface IPrivateAccountsService
    {
        Task<PrivateGetAccountsResponseModel> GetAccounts(string organizationId, string jwtAssertion);

        Task<PrivateGetAccountResponseModel> GetAccount(string organizationId, string jwtAssertion, string accountId);

        Task<PrivateGetAccountTransactionsResponseModel> GetAccountTransactions(
            string organizationId,
            string jwtAssertion,
            string accountId,
            string? paginatingKey,
            string? paginatingSize);
    }
}