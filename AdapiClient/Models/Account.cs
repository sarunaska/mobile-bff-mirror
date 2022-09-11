namespace AdapiClient.Models
{
    public class Account
    {
        public Identifications? Identifications { get; set; }
        public string? Name { get; set; }
        public string? Currency { get; set; }
        public Product? Product { get; set; }
        public Balance[]? Balances { get; set; }
        public Alias[]? Aliases { get; set; }
    }
}
