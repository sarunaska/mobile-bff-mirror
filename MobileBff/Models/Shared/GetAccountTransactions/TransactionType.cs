namespace MobileBff.Models.Shared.GetAccountTransactions
{
    public enum TransactionType
    {
        CardTransaction,
        Payment,
        Transfer,
        RecurringTransfer,
        InternationalPayment,
        SwishPayment,
        BankgiroDeposit,
        Other
    }
}
