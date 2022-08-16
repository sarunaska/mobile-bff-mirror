using MobileBff.Models.Corporate.GetAccount;
using MobileBff.Models.Corporate.GetAccounts;
using MobileBff.Models.Corporate.GetAccountTransactions;

namespace MobileBff.Services
{
    public interface ICorporateAccountsService
    {
        Task<CorporateGetAccountsResponseModel> GetAccounts(string organizationId, string jwtAssertion);

        Task<CorporateGetAccountResponseModel> GetAccount(string organizationId, string jwtAssertion, string accountId);

        Task<CorporateGetAccountTransactionsResponseModel> GetAccountTransactions(
            string organizationId,
            string jwtAssertion,
            string accountId,
            string? paginatingKey,
            string? paginatingSize);
    }
}