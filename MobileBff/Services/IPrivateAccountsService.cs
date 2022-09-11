using MobileBff.Models.Private.GetAccount;
using MobileBff.Models.Private.GetAccountFutureEvents;
using MobileBff.Models.Private.GetAccountReservedAmounts;
using MobileBff.Models.Private.GetAccounts;
using MobileBff.Models.Private.GetAccountTransactions;

namespace MobileBff.Services
{
    public interface IPrivateAccountsService
    {
        Task<PrivateGetAccountsResponseModel> GetAccounts(string userId, string jwtAssertion);

        Task<PrivateGetAccountResponseModel> GetAccount(string userId, string jwtAssertion, string accountId);

        Task<PrivateGetAccountTransactionsResponseModel> GetAccountTransactions(
            string userId,
            string jwtAssertion,
            string accountId,
            string? paginatingKey,
            string? paginatingSize);

        Task<PrivateGetAccountFutureEventsResponseModel> GetAccountFutureEvents(
            string userId,
            string jwtAssertion,
            string accountId);

        Task<PrivateGetAccountReservedAmountsResponseModel> GetAccountReservedAmounts(
            string userId,
            string jwtAssertion,
            string accountId);
    }
}