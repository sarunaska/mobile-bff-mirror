using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccounts;
using MobileBff.Attributes;
using MobileBff.Models.Shared.GetAccounts;

namespace MobileBff.Models.Youth.GetAccounts
{
    public class YouthGetAccountsResponseModel : GetAccountsResponseModel, IPartialResponseModel
    {
        [BffRequired]
        [JsonPropertyName("account_groups")]
        public List<YouthAccountGroupModel>? AccountGroups { get; }

        public YouthGetAccountsResponseModel(GetAccountsResult? result) : base(result)
        {
            AccountGroups = result?.Accounts == null
                ? null
                : new List<YouthAccountGroupModel>
            {
                new YouthAccountGroupModel(result.Accounts)
            };
        }
    }
}