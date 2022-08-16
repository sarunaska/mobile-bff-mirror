#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace AdapiClient.Models
{
    public class PaginatingInformation
    {
        public bool Paginating { get; set; }

        public DateTime? DateFrom { get; set; }

        public string? PaginatingKey { get; set; }
    }
}
