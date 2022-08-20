using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccounts;

namespace MobileBff.Models.Corporate.GetAccounts
{
    public class CorporateGetAccountsResponseModel
    {
        [JsonPropertyName("retrieved_date_time")]
        public DateTime RetrievedDateTime { get; }

        [JsonPropertyName("account_groups")]
        public CorporateAccountGroupModel[] AccountGroups { get; }

        public CorporateGetAccountsResponseModel(GetAccountsResult result)
        {
            RetrievedDateTime = result.RetrievedDateTime;

            var accountsGrouped = result.Accounts.GroupBy(x => x.Currency, (currency, accounts) => new { Currency = currency, Accounts = accounts });
            AccountGroups = accountsGrouped.Select(accountGroup => new CorporateAccountGroupModel(accountGroup.Currency, accountGroup.Accounts)).ToArray();
        }
    }
}