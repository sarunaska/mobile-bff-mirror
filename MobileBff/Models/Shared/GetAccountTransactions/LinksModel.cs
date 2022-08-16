using AdapiClient.Models;

namespace MobileBff.Models.Shared.GetAccountTransactions
{
    public class LinksModel
    {
        public TransactionDetailsModel TransactionDetails { get; }

        public LinksModel(Links links)
        {
            TransactionDetails = new TransactionDetailsModel(links.TransactionDetails);
        }
    }
}
