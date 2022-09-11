using AdapiClient.Builders;
using AdapiClient.Configuration;
using AdapiClient.Endpoints.GetAccount;
using AdapiClient.Endpoints.GetAccountFutureEvents;
using AdapiClient.Endpoints.GetAccountReservedAmounts;
using AdapiClient.Endpoints.GetAccounts;
using AdapiClient.Endpoints.GetAccountTransactions;
using AdapiClient.HttpClients;

namespace AdapiClient
{
    internal class AdapiClient : IAdapiClient
    {
        private readonly IAdapiClientConfiguration configuration;
        private readonly IAdapiHttpClient adapiHttpClient;

        public AdapiClient(IAdapiHttpClient adapiHttpClient, IAdapiClientConfiguration configuration)
        {
            this.configuration = configuration;

            this.adapiHttpClient = adapiHttpClient;
        }

        public async Task<GetAccountsResponse?> GetAccounts(
            string organizationId,
            string jwtAssertion)
        {
            var requestUri = new Uri(configuration.BaseUrl, $"accounts");

            return await adapiHttpClient.MakeGetRequest<GetAccountsResponse>(requestUri, organizationId, jwtAssertion);
        }

        public async Task<GetAccountResponse?> GetAccount(
            string organizationId,
            string jwtAssertion,
            string accountId)
        {
            var requestUri = new Uri(configuration.BaseUrl, $"accounts/{accountId}");

            return await adapiHttpClient.MakeGetRequest<GetAccountResponse>(requestUri, organizationId, jwtAssertion);
        }

        public async Task<GetAccountTransactionsResponse?> GetAccountTransactions(
            string organizationId,
            string jwtAssertion,
            string accountId,
            string? paginatingKey,
            string? paginatingSize)
        {
            var requestUri = new AdapiUriBuilder(configuration.BaseUrl, $"accounts/{accountId}/transactions")
                .AddPagingParameters(paginatingKey, paginatingSize)
                .Build();

            return await adapiHttpClient.MakeGetRequest<GetAccountTransactionsResponse>(requestUri, organizationId, jwtAssertion);
        }

        public async Task<GetAccountFutureEventsResponse?> GetAccountFutureEvents(
            string organizationId,
            string jwtAssertion,
            string accountId)
        {
            var requestUri = new AdapiUriBuilder(configuration.BaseUrl, $"accounts/{accountId}/futureevents")
                .Build();

            return await adapiHttpClient.MakeGetRequest<GetAccountFutureEventsResponse>(requestUri, organizationId, jwtAssertion);
        }

        public async Task<GetAccountReservedAmountsResponse?> GetAccountReservedAmounts(
            string organizationId,
            string jwtAssertion,
            string accountId)
        {
            var requestUri = new AdapiUriBuilder(configuration.BaseUrl, $"accounts/{accountId}/reservedamounts")
                .Build();

            return await adapiHttpClient.MakeGetRequest<GetAccountReservedAmountsResponse>(requestUri, organizationId, jwtAssertion);
        }
    }
}
