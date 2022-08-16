using AdapiClient.Configuration;

namespace MobileBff.Configuration
{
    public class AdapiClientConfiguration : IAdapiClientConfiguration
    {
        public Uri BaseUrl { get; }

        public string ClientId { get; }

        public AdapiClientConfiguration(IConfigurationSection configurationSection)
        {
            BaseUrl = new Uri(configurationSection["Url"]);

            ClientId = configurationSection["ClientId"];
        }
    }
}
