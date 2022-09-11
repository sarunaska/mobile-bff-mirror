using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Attributes;
using MobileBff.ExtensionMethods;

namespace MobileBff.Models.Shared.GetAccountFutureEvents
{
    public class FutureEventModel
    {
        [BffRequired]
        [JsonPropertyName("payment_type")]
        public string? PaymentType { get; }

        [BffRequired]
        [JsonPropertyName("detail_type")]
        public FutureEventDetailTypeModel? DetailType { get; }

        [BffRequired]
        [JsonPropertyName("payment_related_id")]
        public string? PaymentRelatedId { get; }

        [BffRequired]
        [JsonPropertyName("credit_account")]
        public string? CreditAccount { get; }

        [JsonPropertyName("creditor_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CreditorName { get; }

        [BffRequired]
        [JsonPropertyName("debit_account")]
        public string? DebitAccount { get; }

        [BffRequired]
        [JsonPropertyName("value_date")]
        public string? ValueDate { get; }

        [BffRequired]
        [JsonPropertyName("amount")]
        public decimal? Amount { get; }

        [BffRequired]
        [JsonPropertyName("currency")]
        public string? Currency { get; }

        [BffRequired]
        [JsonPropertyName("message")]
        public string? Message { get; }

        [JsonPropertyName("ocr")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Ocr { get; }

        [JsonPropertyName("bank_prefix")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BankPrefix { get; }

        [JsonPropertyName("suti")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Suti { get; }

        [BffRequired]
        [JsonPropertyName("is_equivalent_amount")]
        public bool? IsEquivalentAmount { get; }

        [BffRequired]
        [JsonPropertyName("delete_allowed")]
        public bool? DeleteAllowed { get; }

        [BffRequired]
        [JsonPropertyName("modify_allowed")]
        public bool? ModifyAllowed { get; }

        public FutureEventModel(FutureEvent futureEvent)
        {
            PaymentType = futureEvent.PaymentType;

            DetailType = new FutureEventDetailTypeModel(futureEvent.PaymentType, futureEvent.PaymentDetailType);

            var amount = futureEvent.Amount.ToBankingDecimal();
            if (futureEvent.CreditDebitIndicator == Constants.CreditDebitIndicators.DBIT && amount > 0)
            {
                amount = -amount;
            }

            PaymentRelatedId = futureEvent.PaymentRelatedId;
            CreditAccount = futureEvent.CreditAccount;
            CreditorName = futureEvent.CreditorName;
            DebitAccount = futureEvent.DebitAccount;
            ValueDate = futureEvent.ValueDate;
            Amount = amount;
            Currency = futureEvent.Currency;
            Message = futureEvent.DescriptiveText;
            Ocr = futureEvent.Ocr;
            BankPrefix = futureEvent.BankPrefix;
            Suti = futureEvent.Suti;
            IsEquivalentAmount = futureEvent.IsEquivalentAmount;
            DeleteAllowed = futureEvent.DeleteAllowed;
            ModifyAllowed = futureEvent.ModifyAllowed;
        }
    }
}
