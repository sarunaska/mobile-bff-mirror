using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Models.Shared.GetAccounts;

namespace MobileBff.Models.Private.GetAccounts
{
    public class PrivateAccountGroupModel
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("owner")]
        public OwnerModel? Owner { get; set; }

        [JsonPropertyName("accounts")]
        public PrivateAccountModel[] Accounts { get; set; }

        public PrivateAccountGroupModel(string accountsOwnerUserId, IEnumerable<Account> accounts, bool addOwnerToResponse)
        {
            if (addOwnerToResponse)
            {
                Owner = new OwnerModel(accountsOwnerUserId, null);
            }

            accounts = accounts
                .Where(x => x.Currency == Constants.Currencies.SEK)
                .Where(x => x.Product.ProductCode != Constants.ProductCodes.Ips)
                .ToArray();

            Accounts = accounts.Select(account => new PrivateAccountModel(account)).ToArray();
        }
    }
}