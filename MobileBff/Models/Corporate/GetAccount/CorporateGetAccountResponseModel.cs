using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccount;
using MobileBff.Attributes;
using MobileBff.Models.Shared.GetAccount;

namespace MobileBff.Models.Corporate.GetAccount
{
    public class CorporateGetAccountResponseModel : GetAccountResponseModel, IPartialResponseModel
    {
        [BffRequired]
        [JsonPropertyName("details")]
        public List<CorporateDetailModel>? Details { get; }

        public CorporateGetAccountResponseModel(GetAccountResult? result) : base(result)
        {
            Details = result?.Account == null
                ? null
                : new List<CorporateDetailModel>
            {
                new CorporateDetailModel(result.Account)
            };
        }
    }
}