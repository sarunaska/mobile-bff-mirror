using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccounts;
using MobileBff.Models.Shared.GetAccounts;

namespace MobileBff.Models.Youth.GetAccounts
{
    public class YouthGetAccountsResponseModel
    {
        [JsonPropertyName("retrieved_date_time")]
        public DateTime RetrievedDateTime { get; }

        [JsonPropertyName("account_groups")]
        public AccountGroupModel[] AccountGroups { get; }

        public YouthGetAccountsResponseModel(GetAccountsResult result)
        {
            RetrievedDateTime = result.RetrievedDateTime;

            var accountsGrouped = result.Accounts.GroupBy(x => x.Currency, (currency, accounts) => new { Currency = currency, Accounts = accounts });
            AccountGroups = accountsGrouped.Select(accountGroup => new AccountGroupModel(accountGroup.Accounts)).ToArray();
        }
    }
}