using MobileBff.Resources;

namespace MobileBff.Models.Shared.GetAccount.Items
{
    public class AccountNumberItemModel : ItemModel
    {
        public AccountNumberItemModel(string? value)
            : base(Titles.AccountItem_AccountNumber, value, Constants.Actions.Copy)
        {
        }
    }
}
