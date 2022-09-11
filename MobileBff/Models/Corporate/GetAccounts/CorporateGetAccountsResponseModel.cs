using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccounts;
using MobileBff.Attributes;
using MobileBff.Models.Shared.GetAccounts;

namespace MobileBff.Models.Corporate.GetAccounts
{
    public class CorporateGetAccountsResponseModel : GetAccountsResponseModel, IPartialResponseModel
    {
        [BffRequired]
        [JsonPropertyName("account_groups")]
        public List<CorporateAccountGroupModel>? AccountGroups { get; }

        public CorporateGetAccountsResponseModel(GetAccountsResult? result) : base(result)
        {
            var accountsGrouped = result?.Accounts?
                .GroupBy(x => x.Currency, (currency, accounts) => new
                    {
                        Currency = currency == Constants.Currencies.SEK ? null : currency,
                        Accounts = accounts
                    })
                .OrderBy(x => x.Currency);

            AccountGroups = accountsGrouped?
                .Select(accountGroup => new CorporateAccountGroupModel(accountGroup.Currency, accountGroup.Accounts))
                .ToList();
        }
    }
}