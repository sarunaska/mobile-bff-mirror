using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccount;
using MobileBff.Attributes;
using MobileBff.Models.Shared.GetAccount;
using SebCsClient.Models;

namespace MobileBff.Models.Youth.GetAccount
{
    public class YouthGetAccountResponseModel : GetAccountResponseModel, IPartialResponseModel
    {
        [BffRequired]
        [JsonPropertyName("details")]
        public List<YouthDetailModel>? Details { get; }

        public YouthGetAccountResponseModel(GetAccountResult? result, AccountOwner? accountOwner) : base(result)
        {
            Details = result?.Account == null
                ? null
                : new List<YouthDetailModel>
            {
                new YouthDetailModel(result.Account, accountOwner)
            };
        }
    }
}