using A3SClient.Configuration;

namespace MobileBff.Configurations
{
    public class A3SClientConfiguration : IA3SClientConfiguration
    {
        public Uri BaseUrl { get; }

        public A3SClientConfiguration(IConfigurationSection configurationSection)
        {
            BaseUrl = new Uri(configurationSection["Url"]);
        }
    }
}
