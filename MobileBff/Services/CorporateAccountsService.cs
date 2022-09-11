using AdapiClient;
using MobileBff.Models.Corporate.GetAccount;
using MobileBff.Models.Corporate.GetAccountFutureEvents;
using MobileBff.Models.Corporate.GetAccountReservedAmounts;
using MobileBff.Models.Corporate.GetAccounts;
using MobileBff.Models.Corporate.GetAccountTransactions;

namespace MobileBff.Services
{
    public class CorporateAccountsService : ICorporateAccountsService
    {
        private readonly IAdapiClient adapiClient;

        public CorporateAccountsService(IAdapiClient adapiClient)
        {
            this.adapiClient = adapiClient;
        }

        public async Task<CorporateGetAccountsResponseModel> GetAccounts(string organizationId, string jwtAssertion)
        {
            var apiResponse = await adapiClient.GetAccounts(organizationId, jwtAssertion);

            var response = new CorporateGetAccountsResponseModel(apiResponse?.Result);
            return response;
        }

        public async Task<CorporateGetAccountResponseModel> GetAccount(string organizationId, string jwtAssertion, string accountId)
        {
            var apiResponse = await adapiClient.GetAccount(organizationId, jwtAssertion, accountId);

            var response = new CorporateGetAccountResponseModel(apiResponse?.Result);
            return response;
        }

        public async Task<CorporateGetAccountTransactionsResponseModel> GetAccountTransactions(
            string organizationId,
            string jwtAssertion,
            string accountId,
            string? paginatingKey,
            string? paginatingSize)
        {
            var apiResponse = await adapiClient.GetAccountTransactions(organizationId, jwtAssertion, accountId, paginatingKey, paginatingSize);

            var response = new CorporateGetAccountTransactionsResponseModel(apiResponse?.Result);
            return response;
        }

        public async Task<CorporateGetAccountFutureEventsResponseModel> GetAccountFutureEvents(
            string organizationId,
            string jwtAssertion,
            string accountId)
        {
            var apiResponse = await adapiClient.GetAccountFutureEvents(organizationId, jwtAssertion, accountId);

            var response = new CorporateGetAccountFutureEventsResponseModel(apiResponse?.Result);
            return response;
        }

        public async Task<CorporateGetAccountReservedAmountsResponseModel> GetAccountReservedAmounts(string organizationId, string jwtAssertion, string accountId)
        {
            var apiResponse = await adapiClient.GetAccountReservedAmounts(organizationId, jwtAssertion, accountId);

            var response = new CorporateGetAccountReservedAmountsResponseModel(apiResponse?.Result);
            return response;
        }
    }
}
