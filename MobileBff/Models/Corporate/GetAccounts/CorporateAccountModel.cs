using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Models.Shared.GetAccounts;

namespace MobileBff.Models.Corporate.GetAccounts
{
    public class CorporateAccountModel : AccountModel
    {
        private readonly string[] aliasTypes = new[] { Constants.AliasTypes.Swish, Constants.AliasTypes.BankGiro };

        [JsonPropertyName("aliases")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<AliasModel>? Aliases { get; }

        public CorporateAccountModel(Account account) : base(account)
        {
            Aliases = account.Aliases?
                .Where(x => aliasTypes.Contains(x.Type))
                .Select(alias => new AliasModel(alias))
                .ToList();
        }
    }
}