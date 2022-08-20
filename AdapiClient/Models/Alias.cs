#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace AdapiClient.Models
{
    public class Alias
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string? Name { get; set; }
    }
}
