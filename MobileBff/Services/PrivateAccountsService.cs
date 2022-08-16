using A3SClient;
using AdapiClient;
using AdapiClient.Endpoints.GetAccounts;
using MobileBff.Models;
using MobileBff.Models.Private.GetAccount;
using MobileBff.Models.Private.GetAccounts;
using MobileBff.Models.Private.GetAccountTransactions;

namespace MobileBff.Services
{
    public class PrivateAccountsService : IPrivateAccountsService
    {
        private readonly IAdapiClient adapiClient;
        private readonly IA3SClient a3sClient;

        public PrivateAccountsService(IA3SClient a3sClient, IAdapiClient adapiClient)
        {
            this.a3sClient = a3sClient;
            this.adapiClient = adapiClient;
        }

        public async Task<PrivateGetAccountsResponseModel> GetAccounts(string userId, string jwtAssertion)
        {
            var a3sResponse = await a3sClient.GetUserCustomers(userId, jwtAssertion);

            var userCustomerIds = a3sResponse.Data.UserCustomers
                .Where(x => x.AuthorizationScope.Contains(Constants.AuthorizationScopes.Private))
                .Select(x => x.PublicIdentifier).ToArray();

            var adapiUserAccounts = new Dictionary<string, GetAccountsResult>();

            foreach (var userCustomerId in userCustomerIds)
            {
                var adapiResponse = await adapiClient.GetAccounts(userCustomerId, jwtAssertion);
                adapiUserAccounts.Add(userCustomerId, adapiResponse.Result);
            }

            var response = new PrivateGetAccountsResponseModel(userId, adapiUserAccounts);
            return response;
        }

        public async Task<PrivateGetAccountResponseModel> GetAccount(string organizationId, string jwtAssertion, string accountId)
        {
            var apiResponse = await adapiClient.GetAccount(organizationId, jwtAssertion, accountId);

            var response = new PrivateGetAccountResponseModel(apiResponse.Result);
            return response;
        }

        public async Task<PrivateGetAccountTransactionsResponseModel> GetAccountTransactions(
            string organizationId,
            string jwtAssertion,
            string accountId,
            string? paginatingKey,
            string? paginatingSize)
        {
            var apiResponse = await adapiClient.GetAccountTransactions(organizationId, jwtAssertion, accountId, paginatingKey, paginatingSize);

            var response = new PrivateGetAccountTransactionsResponseModel(apiResponse.Result);
            return response;
        }
    }
}
