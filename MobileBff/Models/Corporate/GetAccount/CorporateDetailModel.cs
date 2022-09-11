using AdapiClient.Models;
using MobileBff.Models.Shared.GetAccount;
using MobileBff.Models.Shared.GetAccount.Items;

namespace MobileBff.Models.Corporate.GetAccount
{
    public class CorporateDetailModel : DetailModel
    {
        public CorporateDetailModel(Account account) : base(account)
        {
            AddItems(account);
        }

        private void AddItems(Account account)
        {
            var swishCorporateNumberAliases = account.Aliases?.Where(x => x.ProductId == Constants.ProductIds.SwishCorporate).Select(x => x).ToArray();
            if (swishCorporateNumberAliases != null && swishCorporateNumberAliases.Any())
            {
                Items.Add(new SwishCorporateNumbersItemModel(swishCorporateNumberAliases));
            }

            var swishForMerchantAliases = account.Aliases?.Where(x => x.ProductId == Constants.ProductIds.SwishForMerchants).Select(x => x).ToArray();
            if (swishForMerchantAliases != null && swishForMerchantAliases.Any())
            {
                Items.Add(new SwishForMerchantsItemModel(swishForMerchantAliases));
            }

            var bankGiroNumbers = account.Aliases?.Where(x => x.Type == Constants.AliasTypes.BankGiro && x.Id != null).Select(x => x.Id!).ToArray();
            if (bankGiroNumbers != null && bankGiroNumbers.Any())
            {
                Items.Add(new BankGiroItemModel(bankGiroNumbers));
            }
        }
    }
}