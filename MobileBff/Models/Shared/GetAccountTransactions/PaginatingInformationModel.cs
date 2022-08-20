using System.Text.Json.Serialization;
using AdapiClient.Models;

namespace MobileBff.Models.Shared.GetAccountTransactions
{
    public class PaginatingInformationModel
    {
        [JsonPropertyName("paginating")]
        public bool Paginating { get; }

        [JsonPropertyName("date_from")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? DateFrom { get; }

        [JsonPropertyName("paginating_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PaginatingKey { get; }

        public PaginatingInformationModel(PaginatingInformation paginatingInformation)
        {
            Paginating = paginatingInformation.Paginating;
            PaginatingKey = paginatingInformation.PaginatingKey;
            DateFrom = paginatingInformation.DateFrom;
        }
    }
}
