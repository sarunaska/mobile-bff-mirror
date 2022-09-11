using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccount;
using MobileBff.Attributes;
using MobileBff.Models.Shared.GetAccount;
using SebCsClient.Models;

namespace MobileBff.Models.Private.GetAccount
{
    public class PrivateGetAccountResponseModel : GetAccountResponseModel, IPartialResponseModel
    {
        [BffRequired]
        [JsonPropertyName("details")]
        public List<PrivateDetailModel>? Details { get; }

        public PrivateGetAccountResponseModel(GetAccountResult? result, AccountOwner? accountOwner) : base(result)
        {
            Details = result?.Account == null
                ? null
                : new List<PrivateDetailModel>
            {
                new PrivateDetailModel(result.Account, accountOwner)
            };
        }
    }
}