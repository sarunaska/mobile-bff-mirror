using System.Text.Json.Serialization;
using AdapiClient.Models;

namespace MobileBff.Models.Corporate.GetAccounts
{
    public class CorporateAccountGroupModel
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Currency { get; set; }

        [JsonPropertyName("accounts")]
        public CorporateAccountModel[] Accounts { get; set; }

        public CorporateAccountGroupModel(string currency, IEnumerable<Account> accounts)
        {
            Currency = currency == Constants.Currencies.SEK ? null : currency;
            Accounts = accounts.Select(account => new CorporateAccountModel(account)).ToArray();
        }
    }
}