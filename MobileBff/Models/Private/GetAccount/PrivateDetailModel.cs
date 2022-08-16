using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Models.Shared.GetAccount;
using MobileBff.Models.Shared.GetAccount.Items;
using MobileBff.Resources;

namespace MobileBff.Models.Private.GetAccount
{
    public class PrivateDetailModel
    {
        public PrivateDetailModel(Account account)
        {
            Title = Titles.Account_Details;
            Items = GetItems(account);
        }

        [JsonPropertyName("title")]
        public string Title { get; }

        [JsonPropertyName("items")]
        public ItemModel[] Items { get; }

        private static ItemModel[] GetItems(Account account)
        {
            var items = new List<ItemModel>();

            items.Add(new AccountNameItemModel(account.Name ?? account.Product.Name));
            items.Add(new AccountTypeItemModel(account.Product.Name));
            items.Add(new AccountNumberItemModel(account.Identifications.DomesticAccountNumber));
            items.Add(new AccountOwnerItemModel(null)); // Owner name from Kurre, TBD how to retrieve it.

            items.Add(new IbanItemModel(account.Identifications.Iban));
            items.Add(new BicItemModel(account.Identifications.Bic));

            return items.ToArray();
        }
    }
}