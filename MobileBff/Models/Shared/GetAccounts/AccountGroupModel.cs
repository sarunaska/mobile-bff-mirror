using System.Text.Json.Serialization;
using AdapiClient.Models;

namespace MobileBff.Models.Shared.GetAccounts
{
    public class AccountGroupModel
    {
        [JsonPropertyName("accounts")]
        public List<AccountModel> Accounts { get; }

        public AccountGroupModel(IEnumerable<Account> accounts)
        {
            Accounts = accounts.Select(account => new AccountModel(account)).ToList();
        }
    }
}