using AdapiClient;
using MobileBff.Models.Youth.GetAccount;
using MobileBff.Models.Youth.GetAccountReservedAmounts;
using MobileBff.Models.Youth.GetAccounts;
using MobileBff.Models.Youth.GetAccountTransactions;
using SebCsClient;

namespace MobileBff.Services
{
    public class YouthAccountsService : IYouthAccountsService
    {
        private readonly IAdapiClient adapiClient;
        private readonly ISebCsClient sebCsClient;

        public YouthAccountsService(IAdapiClient adapiClient, ISebCsClient sebCsClient)
        {
            this.adapiClient = adapiClient;
            this.sebCsClient = sebCsClient;
        }

        public async Task<YouthGetAccountsResponseModel> GetAccounts(string userId, string jwtAssertion)
        {
            var adapiResponse = await adapiClient.GetAccounts(userId, jwtAssertion);

            var response = new YouthGetAccountsResponseModel(adapiResponse?.Result);
            return response;
        }

        public async Task<YouthGetAccountResponseModel> GetAccount(string userId, string jwtAssertion, string accountId)
        {
            var adapiResponse = await adapiClient.GetAccount(userId, jwtAssertion, accountId);

            var accountOwner = await sebCsClient.GetAccountOwner(userId, jwtAssertion);

            var response = new YouthGetAccountResponseModel(adapiResponse?.Result, accountOwner);
            return response;
        }

        public async Task<YouthGetAccountTransactionsResponseModel> GetAccountTransactions(
            string userId,
            string jwtAssertion,
            string accountId,
            string? paginatingKey,
            string? paginatingSize)
        {
            var adapiResponse = await adapiClient.GetAccountTransactions(userId, jwtAssertion, accountId, paginatingKey, paginatingSize);

            var response = new YouthGetAccountTransactionsResponseModel(adapiResponse?.Result);
            return response;
        }

        public async Task<YouthGetAccountReservedAmountsResponseModel> GetAccountReservedAmounts(string userId, string jwtAssertion, string accountId)
        {
            var apiResponse = await adapiClient.GetAccountReservedAmounts(userId, jwtAssertion, accountId);

            var response = new YouthGetAccountReservedAmountsResponseModel(apiResponse?.Result);
            return response;
        }
    }
}
