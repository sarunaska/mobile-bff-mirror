#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace AdapiClient.Models
{
    public class Links
    {
        public string Type { get; set; }

        public TransactionDetails TransactionDetails { get; set; }
    }
}
