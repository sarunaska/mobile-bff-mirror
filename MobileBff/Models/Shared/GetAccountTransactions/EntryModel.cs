using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Attributes;
using MobileBff.ExtensionMethods;

namespace MobileBff.Models.Shared.GetAccountTransactions
{
    public class EntryModel
    {
        [BffRequired]
        [JsonPropertyName("transaction_id")]
        public string? TransactionId { get; }

        [BffRequired]
        [JsonPropertyName("value_date")]
        public string? ValueDate { get; }

        [BffRequired]
        [JsonPropertyName("booking_date")]
        public string? BookingDate { get; }

        [BffRequired]
        [JsonPropertyName("amount")]
        public decimal? Amount { get; }

        [BffRequired]
        [JsonPropertyName("booked_balance")]
        public decimal? BookedBalance { get; }

        [BffRequired]
        [JsonPropertyName("currency")]
        public string? Currency { get; }

        [BffRequired]
        [JsonPropertyName("message")]
        public string? Message { get; }

        [JsonPropertyName("type")]
        public TypeModel Type { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("_links")]
        public LinksModel? Links { get; }

        public EntryModel(BookingEntry bookingEntry)
        {
            TransactionId = bookingEntry.TransactionId;
            ValueDate = bookingEntry.ValueDate;
            BookingDate = bookingEntry.BookingDate;
            Amount = bookingEntry.TransactionAmount?.Amount?.ToBankingDecimal();
            BookedBalance = bookingEntry.BookedBalance?.Amount?.ToBankingDecimal();
            Currency = bookingEntry.TransactionAmount?.Currency;

            var transactionType = GetTransactionType(bookingEntry.BankTransactionCode?.EntryType);

            Type = new TypeModel(bookingEntry.BankTransactionCode, transactionType);

            Message = transactionType == TransactionType.CardTransaction
                ? bookingEntry.CardBookingEntryDetails?.MerchantName
                : bookingEntry.Message1;

            Links = bookingEntry.Links == null ? null : new LinksModel(bookingEntry.Links);
        }

        private static TransactionType GetTransactionType(string? transactionTypeCode)
        {
            switch (transactionTypeCode)
            {
                case "021":
                case "051":
                    return TransactionType.CardTransaction;
                case "080":
                case "081":
                case "082":
                    return TransactionType.Payment;
                case "181":
                case "182":
                case "183":
                case "185":
                    return TransactionType.Transfer;
                case "184":
                    return TransactionType.RecurringTransfer;
                case "211":
                case "212":
                case "213":
                case "214":
                case "221":
                case "222":
                case "223":
                case "224":
                    return TransactionType.InternationalPayment;
                case "141":
                case "142":
                case "143":
                    return TransactionType.SwishPayment;
                case "160":
                    return TransactionType.BankgiroDeposit;
                default:
                    return TransactionType.Other;
            }
        }
    }
}
