using System.Text.Json.Serialization;
using AdapiClient.Models;
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

        [JsonPropertyName("title")]
        public string Title { get; }

        [JsonPropertyName("items")]
        public ItemModel[] Items { get; }

        private static ItemModel[] GetItems(Account account)
        {
            var items = new List<ItemModel>();

            items.Add(new AccountTypeItemModel(account.Product.Name));
            items.Add(new AccountNumberItemModel(account.Identifications.DomesticAccountNumber));

            var swishCorporateNumberAliases = account.Aliases?.Where(x => x.ProductId == Constants.ProductIds.SwishCorporate).Select(x => x).ToArray();
            if (swishCorporateNumberAliases != null && swishCorporateNumberAliases.Any())
            {
                items.Add(new SwishCorporateNumbersItemModel(swishCorporateNumberAliases));
            }

            var swishForMerchantAliases = account.Aliases?.Where(x => x.ProductId == Constants.ProductIds.SwishForMerchants).Select(x => x).ToArray();
            if (swishForMerchantAliases != null && swishForMerchantAliases.Any())
            {
                items.Add(new SwishForMerchantsItemModel(swishForMerchantAliases));
            }

            var bankGiroNumbers = account.Aliases?.Where(x => x.Type == Constants.AliasTypes.BankGiro).Select(x => x.Id).ToArray();
            if (bankGiroNumbers != null && bankGiroNumbers.Any())
            {
                items.Add(new BankGiroItemModel(bankGiroNumbers));
            }

            items.Add(new IbanItemModel(account.Identifications.Iban));
            items.Add(new BicItemModel(account.Identifications.Bic));

            return items.ToArray();
        }
    }
}