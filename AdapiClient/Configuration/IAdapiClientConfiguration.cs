namespace AdapiClient.Configuration
{
    public interface IAdapiClientConfiguration
    {
        public Uri BaseUrl { get; }

        public string ClientId { get; }
    }
}
