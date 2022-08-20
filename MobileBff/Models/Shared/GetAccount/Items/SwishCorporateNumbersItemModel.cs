using AdapiClient.Models;
using MobileBff.Resources;

namespace MobileBff.Models.Shared.GetAccount.Items
{
    public class SwishCorporateNumbersItemModel : SwishValueItemModel
    {
        public SwishCorporateNumbersItemModel(Alias[] aliases)
            : base(Titles.AccountItem_SwishCorporate, aliases)
        {
        }
    }
}
