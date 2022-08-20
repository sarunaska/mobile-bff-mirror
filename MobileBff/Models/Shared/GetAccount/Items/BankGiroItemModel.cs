using MobileBff.Models.Shared.GetAccount;
using MobileBff.Resources;

namespace MobileBff.Models.Shared.GetAccount.Items
{
    public class BankGiroItemModel : ItemModel
    {
        public BankGiroItemModel(string[] valueList)
            : base(Titles.AccountItem_BankGiro, valueList)
        {
        }
    }
}
