using MobileBff.Resources;

namespace MobileBff.Models.Shared.GetAccount.Items
{
    public class AccountOwnerItemModel : ItemModel
    {
        public AccountOwnerItemModel(string value)
            : base(Titles.AccountItem_AccountOwner, value)
        {
        }
    }
}
