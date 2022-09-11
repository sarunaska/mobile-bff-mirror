using System.Text.Json.Serialization;
using AdapiClient.Models;
using MobileBff.Attributes;

namespace MobileBff.Models.Shared.GetAccountTransactions
{
    public class PaginatingInformationModel : ICustomPartialResponse
    {
        [BffRequired]
        [JsonPropertyName("paginating")]
        public bool? Paginating { get; }

        [JsonPropertyName("date_from")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? DateFrom { get; }

        [JsonPropertyName("paginating_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PaginatingKey { get; }

        [JsonIgnore]
        public bool IsPartialResponse
        {
            get
            {
                return Paginating == true && PaginatingKey == null;
            }
        }

        public PaginatingInformationModel(PaginatingInformation? paginatingInformation)
        {
            Paginating = paginatingInformation?.Paginating;
            PaginatingKey = paginatingInformation?.PaginatingKey;
            DateFrom = paginatingInformation?.DateFrom;
        }
    }
}
