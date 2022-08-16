using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Models.Shared.GetAccounts;

namespace MobileBff.Models.Private.GetAccounts
{
    public class PrivateAccountModel : AccountModel
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("hide_in_overview")]
        public bool? HideInOverview { get; }

        public PrivateAccountModel(Account account) : base(account)
        {
            if (account.Product.ProductCode == Constants.ProductCodes.IskDepositAccounts)
            {
                HideInOverview = true;
            }
        }
    }
}