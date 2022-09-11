using AdapiClient.Models;
using MobileBff.Models.Shared.GetAccount;
using MobileBff.Models.Shared.GetAccount.Items;
using SebCsClient.Models;

namespace MobileBff.Models.Private.GetAccount
{
    public class PrivateDetailModel : DetailModel
    {
        public PrivateDetailModel(Account account, AccountOwner? accountOwner) : base(account)
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