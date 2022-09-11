using System.Text.Json.Serialization;
using MobileBff.Attributes;
using MobileBff.Resources;

namespace MobileBff.Models.Shared.GetAccountFutureEvents
{
    public class FutureEventDetailTypeModel
    {
        [BffRequired]
        [JsonPropertyName("code")]
        public string? Code { get; }

        [BffRequired]
        [JsonPropertyName("text")]
        public string Text { get; }

        public FutureEventDetailTypeModel(string? paymentType, string? code)
        {
            Code = code;
            Text = GetFutureEventDetailTypeName(paymentType, code);
        }

        private static string GetFutureEventDetailTypeName(string? paymentType, string? code)
        {
            if (paymentType == Constants.FutureEventPaymentTypes.International)
            {
                return Titles.FutureEventDetailType_InternationalPayment;
            }

            return code switch
            {
                "A" => Titles.FutureEventDetailType_Fee,
                "B" or "F" or "S" => Titles.FutureEventDetailType_Payment,
                "C" or "I" => Titles.FutureEventDetailType_EInvoice,
                "E" or "e" => Titles.FutureEventDetailType_Transfer,
                "O" => Titles.FutureEventDetailType_RecurringTransfer,
                "D" or "L" => Titles.FutureEventDetailType_LoanPayment,
                "K" => Titles.FutureEventDetailType_ProcessingNewEvent,
                "Z" => Titles.FutureEventDetailType_Incomming,
                "X" => Titles.FutureEventDetailType_Unknown,
                "Y" => Titles.FutureEventDetailType_Salary,
                _ => Titles.FutureEventDetailType_Other,
            };
        }
    }
}
