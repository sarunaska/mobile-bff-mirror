using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Attributes;
using MobileBff.Models.Shared.GetAccounts;

namespace MobileBff.Models.Youth.GetAccounts
{
    public class YouthAccountGroupModel
    {
        [BffRequired]
        [JsonPropertyName("accounts")]
        public List<AccountModel> Accounts { get; }

        public YouthAccountGroupModel(IEnumerable<Account> accounts)
        {
            accounts = accounts
                .Where(x => x.Currency == Constants.Currencies.SEK)
                .Where(x => x.Product?.ProductCode != Constants.ProductCodes.Ips)
                .Where(x => x.Product?.ProductCode != Constants.ProductCodes.IskDepositAccounts)
                .ToArray();

            Accounts = accounts.Select(account => new AccountModel(account)).ToList();
        }
    }
}