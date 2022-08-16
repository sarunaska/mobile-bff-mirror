#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace A3SClient.Models
{
    public class UserCustomer
    {
        public string PublicIdentifier { get; set; }

        public string[] AuthorizationScope { get; set; }
    }
}
