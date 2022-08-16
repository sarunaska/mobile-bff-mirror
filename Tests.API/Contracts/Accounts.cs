using System.Text.Json.Serialization;

namespace Tests.API.Contracts
{
    public class AccountsResult
    {
        [JsonPropertyName("retrieved_date_time")] 
        public DateTime? ReceivedDateTime { get; set; }

        [JsonPropertyName("account_groups")] 
        public ICollection<AccountGroup> AccountGroups { get; set; }
    }

    public class AccountGroup
    {
        [JsonPropertyName("currency")] 
        public string? Currency { get; set; }

        [JsonPropertyName("accounts")] 
        public ICollection<Account> Accounts { get; set; }
    }

    public class Account
    {
        [JsonPropertyName("aliases")]
        public ICollection<Alias>? Aliases { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("product_id")]
        public string ProductId { get; set; }

        [JsonPropertyName("resource_id")]
        public string ResourceId { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("balance")]
        public string Balance { get; set; }

        [JsonPropertyName("available")]
        public string Available { get; set; }

        [JsonPropertyName("can_withdraw")]
        public bool CanWithdraw { get; set; }

        [JsonPropertyName("can_deposit")]
        public bool CanDeposit { get; set; }
    }
    
    public class Alias
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("type")]
        public AliasTypes Type { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public enum AliasTypes
    {
        bg,
        swish
    }
}