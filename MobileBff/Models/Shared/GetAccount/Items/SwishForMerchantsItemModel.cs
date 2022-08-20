using AdapiClient.Models;
using MobileBff.Resources;

namespace MobileBff.Models.Shared.GetAccount.Items
{
    public class SwishForMerchantsItemModel : SwishValueItemModel
    {
        public SwishForMerchantsItemModel(Alias[] aliases)
            : base(Titles.AccountItem_SwishForMerchants, aliases)
        {
        }
    }
}
