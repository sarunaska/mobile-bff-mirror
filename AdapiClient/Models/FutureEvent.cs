namespace AdapiClient.Models
{
    public class FutureEvent
    {
        public string? PaymentType { get; set; }
        public string? PaymentDetailType { get; set; }
        public string? PaymentRelatedId { get; set; }
        public string? CreditAccount { get; set; }
        public string? CreditorName { get; set; }
        public string? DebitAccount { get; set; }
        public string? ValueDate { get; set; }
        public string? CreditDebitIndicator { get; set; }
        public string? Amount { get; set; }
        public string? Currency { get; set; }
        public string? DescriptiveText { get; set; }
        public string? Ocr { get; set; }
        public string? BankPrefix { get; set; }
        public string? Suti { get; set; }
        public bool? IsEquivalentAmount { get; set; }
        public bool? DeleteAllowed { get; set; }
        public bool? ModifyAllowed { get; set; }
    }
}
