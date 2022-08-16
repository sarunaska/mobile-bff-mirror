using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Resources;

namespace MobileBff.Models.Shared.GetAccountTransactions
{
    public class TypeModel
    {
        [JsonPropertyName("code")]
        public string Code { get; }

        [JsonPropertyName("text")]
        public string Text { get; }

        public TypeModel(BankTransactionCode bankTransactionCode, TransactionType transactionType)
        {
            Code = bankTransactionCode.EntryType;
            Text = GetTransationTypeName(transactionType);
        }

        private static string GetTransationTypeName(TransactionType transactionType)
        {
            switch (transactionType)
            {
                case TransactionType.CardTransaction:
                    return Titles.TransactionType_CardTransaction;
                case TransactionType.Payment:
                    return Titles.TransactionType_Payment;
                case TransactionType.Transfer:
                    return Titles.TransactionType_Transfer;
                case TransactionType.RecurringTransfer:
                    return Titles.TransactionType_RecurringTransfer;
                case TransactionType.InternationalPayment:
                    return Titles.TransactionType_InternationalPayment;
                case TransactionType.SwishPayment:
                    return Titles.TransactionType_SwishPayment;
                case TransactionType.BankgiroDeposit:
                    return Titles.TransactionType_BankgiroDeposit;
                case TransactionType.Other:
                    return Titles.TransactionType_Other;
                default:
                    throw new Exception($"Unknown transaction type {transactionType}");
            }
        }
    }
}
