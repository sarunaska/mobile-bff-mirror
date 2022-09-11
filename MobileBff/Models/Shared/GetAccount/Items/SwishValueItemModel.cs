using AdapiClient.Models;
using MobileBff.Formatters;

namespace MobileBff.Models.Shared.GetAccount.Items
{
    public class SwishValueItemModel : ItemModel
    {
        public SwishValueItemModel(string title, Alias[] aliases)
            : base(title, GetFormattedSwishCorporateNumbers(aliases))
        {
        }

        private static string?[] GetFormattedSwishCorporateNumbers(Alias[] aliases)
        {
            return aliases
                .Select(x => SwissValueFormatter.Format(x.Id, x.Name))
                .ToArray();
        }
    }
}
