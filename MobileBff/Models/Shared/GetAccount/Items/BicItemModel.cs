using MobileBff.Models.Shared.GetAccount;

namespace MobileBff.Models.Shared.GetAccount.Items
{
    public class BicItemModel : ItemModel
    {
        public BicItemModel(string value)
            : base(Constants.Titles.Bic, value, Constants.Actions.Copy)
        {
        }
    }
}
