using SebCsClient.Configuration;

namespace MobileBff.Configurations
{
    public class SebCsClientConfiguration : ISebCsClientConfiguration
    {
        public Uri BaseUrl { get; }

        public SebCsClientConfiguration(IConfigurationSection configurationSection)
        {
            BaseUrl = new Uri(configurationSection["Url"]);
        }
    }
}
