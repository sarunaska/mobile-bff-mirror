using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Attributes;
using MobileBff.ExtensionMethods;

namespace MobileBff.Models.Shared.GetAccounts
{
    public class AccountModel
    {
        [BffRequired]
        [JsonPropertyName("name")]
        public string? Name { get; }

        [BffRequired]
        [JsonPropertyName("product_id")]
        public string? ProductId { get; }

        [BffRequired]
        [JsonPropertyName("resource_id")]
        public string? ResourceId { get; }

        [BffRequired]
        [JsonPropertyName("account_number")]
        public string? AccountNumber { get; }

        [BffRequired]
        [JsonPropertyName("balance")]
        public decimal? Balance { get; }

        [BffRequired]
        [JsonPropertyName("available")]
        public decimal? Available { get; }

        [BffRequired]
        [JsonPropertyName("can_withdraw")]
        public bool? CanWithdraw { get; }

        [BffRequired]
        [JsonPropertyName("can_deposit")]
        public bool? CanDeposit { get; }

        public AccountModel(Account account)
        {
            Name = account.Name ?? account.Product?.Name;
            ProductId = account.Product?.Id;

            ResourceId = account.Identifications?.ResourceId;
            AccountNumber = account.Identifications?.DomesticAccountNumber;

            var balanceAmount = account.Balances?.SingleOrDefault(x => x.Type == Constants.BalanceTypes.Booked)?.Amount;
            Balance = balanceAmount?.ToBankingDecimal();

            var availableAmount = account.Balances?.SingleOrDefault(x => x.Type == Constants.BalanceTypes.Available)?.Amount;
            Available = availableAmount?.ToBankingDecimal();

            CanWithdraw = true; // Not yet determined where to fetch this flag
            CanDeposit = true; // Not yet determined where to fetch this flag
        }
    }
}