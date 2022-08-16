#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace AdapiClient.Models
{
    public class Identifications
    {
        public string ResourceId { get; set; }
        public string DomesticAccountNumber { get; set; }
        public string Iban { get; set; }
        public string Bic { get; set; }
    }
}
