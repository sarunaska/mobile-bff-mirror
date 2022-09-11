using MobileBff.Resources;

namespace MobileBff.Models.Shared.GetAccount.Items
{
    public class AccountNameItemModel : ItemModel
    {
        public AccountNameItemModel(string? value) : base(Titles.AccountItem_AccountName, value, Constants.Actions.ChangeAccountName)
        {
        }
    }
}
