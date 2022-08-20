﻿using AdapiClient.Endpoints.GetAccount;
using AdapiClient.Endpoints.GetAccounts;
using AdapiClient.Endpoints.GetAccountTransactions;

namespace AdapiClient
{
    public interface IAdapiClient
    {
        Task<GetAccountsResponse> GetAccounts(string organizationId, string jwtAssertion);

        Task<GetAccountResponse> GetAccount(string organizationId, string jwtAssertion, string accountId);

        Task<GetAccountTransactionsResponse> GetAccountTransactions(
            string organizationId,
            string jwtAssertion,
            string accountId,
            string? paginatingKey,
            string? paginatingSize);
    }
}