using MobileBff.Models.Youth.GetAccount;
using MobileBff.Models.Youth.GetAccountReservedAmounts;
using MobileBff.Models.Youth.GetAccounts;
using MobileBff.Models.Youth.GetAccountTransactions;

namespace MobileBff.Services
{
    public interface IYouthAccountsService
    {
        Task<YouthGetAccountResponseModel> GetAccount(string userId, string jwtAssertion, string accountId);

        Task<YouthGetAccountsResponseModel> GetAccounts(string userId, string jwtAssertion);

        Task<YouthGetAccountTransactionsResponseModel> GetAccountTransactions(
            string userId,
            string jwtAssertion,
            string accountId,
            string? paginatingKey,
            string? paginatingSize);

        Task<YouthGetAccountReservedAmountsResponseModel> GetAccountReservedAmounts(
            string userId,
            string jwtAssertion,
            string accountId);
    }
}