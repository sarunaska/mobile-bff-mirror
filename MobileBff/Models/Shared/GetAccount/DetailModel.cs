using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Attributes;
using MobileBff.Models.Shared.GetAccount.Items;
using MobileBff.Resources;

namespace MobileBff.Models.Shared.GetAccount
{
    public class DetailModel
    {
        public DetailModel(Account account)
        {
            Title = Titles.Account_Details;
            Items = GetItems(account);
        }

        [BffRequired]
        [JsonPropertyName("title")]
        public string Title { get; }

        [BffRequired]
        [JsonPropertyName("items")]
        public List<ItemModel> Items { get; }

        private static List<ItemModel> GetItems(Account account)
        {
            var items = new List<ItemModel>
            {
                new AccountTypeItemModel(account.Product?.Name),
                new AccountNumberItemModel(account.Identifications?.DomesticAccountNumber),
                new IbanItemModel(account.Identifications?.Iban),
                new BicItemModel(account.Identifications?.Bic)
            };

            return items;
        }
    }
}