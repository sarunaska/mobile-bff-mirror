using System.Text.Json.Serialization;
using AdapiClient.Models;

namespace MobileBff.Models.Shared.GetAccounts
{
    public class AccountModel
    {
        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("product_id")]
        public string ProductId { get; }

        [JsonPropertyName("resource_id")]
        public string ResourceId { get; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; }

        [JsonPropertyName("balance")]
        public string Balance { get; }

        [JsonPropertyName("available")]
        public string Available { get; }

        [JsonPropertyName("can_withdraw")]
        public bool CanWithdraw { get; }

        [JsonPropertyName("can_deposit")]
        public bool CanDeposit { get; }

        public AccountModel(Account account)
        {
            Name = account.Name ?? account.Product.Name;
            ProductId = account.Product.Id;

            ResourceId = account.Identifications.ResourceId;
            AccountNumber = account.Identifications.DomesticAccountNumber;

            Balance = account.Balances.Single(x => x.Type == Constants.BalanceTypes.Booked).Amount;
            Available = account.Balances.Single(x => x.Type == Constants.BalanceTypes.Available).Amount;

            CanWithdraw = true; // Not yet determined where to fetch this flag
            CanDeposit = true; // Not yet determined where to fetch this flag
        }
    }
}