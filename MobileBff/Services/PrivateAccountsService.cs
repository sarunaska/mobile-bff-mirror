using A3SClient;
using AdapiClient;
using AdapiClient.Endpoints.GetAccounts;
using MobileBff.Models.Private.GetAccount;
using MobileBff.Models.Private.GetAccountFutureEvents;
using MobileBff.Models.Private.GetAccountReservedAmounts;
using MobileBff.Models.Private.GetAccounts;
using MobileBff.Models.Private.GetAccountTransactions;
using MobileBff.Utilities;
using SebCsClient;
using SebCsClient.Models;

namespace MobileBff.Services
{
    public class PrivateAccountsService : IPrivateAccountsService
    {
        private readonly IAdapiClient adapiClient;
        private readonly IA3SClient a3sClient;
        private readonly ISebCsClient sebCsClient;

        public PrivateAccountsService(IA3SClient a3sClient, IAdapiClient adapiClient, ISebCsClient sebCsClient)
        {
            this.a3sClient = a3sClient;
            this.adapiClient = adapiClient;
            this.sebCsClient = sebCsClient;
        }

        public async Task<PrivateGetAccountsResponseModel> GetAccounts(string userId, string jwtAssertion)
        {
            var trimmedUserId = SebCustomerNumberHelper.ConvertToTrimmedSebCustomerNumber(userId);
            var a3sResponse = await a3sClient.GetUserCustomers(trimmedUserId, jwtAssertion);

            var userCustomerIds = a3sResponse?.Data?.UserCustomers
                .Where(x => x.AuthorizationScope != null && x.AuthorizationScope.Contains(Constants.AuthorizationScopes.Private))
                .Where(x => x.PublicIdentifier != null)
                .Select(x => x.PublicIdentifier!)
                .ToArray();

            var userAccounts = new List<(AccountOwner?, GetAccountsResult)>();

            if (userCustomerIds != null)
            {
                foreach (var userCustomerId in userCustomerIds)
                {
                    var adapiResponse = await adapiClient.GetAccounts(userCustomerId, jwtAssertion);
                    if (adapiResponse?.Result == null)
                    {
                        continue;
                    }

                    var accountOwner = userCustomerId == trimmedUserId
                        ? null
                        : await sebCsClient.GetAccountOwner(userCustomerId, jwtAssertion);

                    userAccounts.Add((accountOwner, adapiResponse.Result));
                    }
                }

            var response = new PrivateGetAccountsResponseModel(userAccounts);
            return response;
        }

        public async Task<PrivateGetAccountResponseModel> GetAccount(string userId, string jwtAssertion, string accountId)
        {
            var adapiResponse = await adapiClient.GetAccount(userId, jwtAssertion, accountId);

            var accountOwner = await sebCsClient.GetAccountOwner(userId, jwtAssertion);

            var response = new PrivateGetAccountResponseModel(adapiResponse?.Result, accountOwner);
            return response;
        }

        public async Task<PrivateGetAccountTransactionsResponseModel> GetAccountTransactions(
            string userId,
            string jwtAssertion,
            string accountId,
            string? paginatingKey,
            string? paginatingSize)
        {
            var adapiResponse = await adapiClient.GetAccountTransactions(userId, jwtAssertion, accountId, paginatingKey, paginatingSize);

            var response = new PrivateGetAccountTransactionsResponseModel(adapiResponse?.Result);
            return response;
        }

        public async Task<PrivateGetAccountFutureEventsResponseModel> GetAccountFutureEvents(
            string userId,
            string jwtAssertion,
            string accountId)
        {
            var apiResponse = await adapiClient.GetAccountFutureEvents(userId, jwtAssertion, accountId);

            var response = new PrivateGetAccountFutureEventsResponseModel(apiResponse?.Result);
            return response;
        }

        public async Task<PrivateGetAccountReservedAmountsResponseModel> GetAccountReservedAmounts(string userId, string jwtAssertion, string accountId)
        {
            var apiResponse = await adapiClient.GetAccountReservedAmounts(userId, jwtAssertion, accountId);

            var response = new PrivateGetAccountReservedAmountsResponseModel(apiResponse?.Result);
            return response;
        }
    }
}
