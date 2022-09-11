using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Attributes;

namespace MobileBff.Models.Corporate.GetAccounts
{
    public class CorporateAccountGroupModel
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Currency { get; }

        [BffRequired]
        [JsonPropertyName("accounts")]
        public List<CorporateAccountModel>? Accounts { get; }

        public CorporateAccountGroupModel(string? currency, IEnumerable<Account> accounts)
        {
            Currency = currency;
            Accounts = accounts.Select(account => new CorporateAccountModel(account)).ToList();
        }
    }
}