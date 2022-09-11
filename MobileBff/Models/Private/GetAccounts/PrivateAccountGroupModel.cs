using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Attributes;
using MobileBff.Models.Shared.GetAccounts;
using SebCsClient.Models;

namespace MobileBff.Models.Private.GetAccounts
{
    public class PrivateAccountGroupModel
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("owner")]
        public OwnerModel? Owner { get; set; }

        [BffRequired]
        [JsonPropertyName("accounts")]
        public List<PrivateAccountModel>? Accounts { get; set; }

        public PrivateAccountGroupModel(AccountOwner? accountOwner, Account[]? accounts)
        {
            if (accountOwner != null)
            {
                Owner = new OwnerModel(accountOwner);
            }

            accounts = accounts?
                .Where(x => x.Currency == Constants.Currencies.SEK)
                .Where(x => x.Product?.ProductCode != Constants.ProductCodes.Ips)
                .ToArray();

            Accounts = accounts?.Select(account => new PrivateAccountModel(account)).ToList();
        }
    }
}