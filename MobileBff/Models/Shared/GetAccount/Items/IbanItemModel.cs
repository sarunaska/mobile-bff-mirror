using MobileBff.Formatters;

namespace MobileBff.Models.Shared.GetAccount.Items
{
    public class IbanItemModel : ItemModel
    {
        public IbanItemModel(string? value)
            : base(Constants.Titles.Iban, IbanFormatter.Format(value), Constants.Actions.Copy)
        {
        }
    }
}
