using AdapiClient.Models;
using MobileBff.Models.Shared.GetAccount;
using MobileBff.Models.Shared.GetAccount.Items;
using SebCsClient.Models;

namespace MobileBff.Models.Youth.GetAccount
{
    public class YouthDetailModel : DetailModel
    {
        public YouthDetailModel(Account account, AccountOwner? accountOwner) : base(account)
        {
            AddItems(account, accountOwner);
        }

        private void AddItems(Account account, AccountOwner? accountOwner)
        {
            Items.Add(new AccountNameItemModel(account.Name ?? account.Product?.Name));

            if (accountOwner?.Name != null)
            {
                Items.Add(new AccountOwnerItemModel(accountOwner.Name));
        }
    }
    }
}