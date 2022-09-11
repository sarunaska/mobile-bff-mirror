using AdapiClient.Configuration;
using AdapiClient.HttpClients;
using Microsoft.Extensions.DependencyInjection;

namespace AdapiClient.Extensions
{
    public static class AdapiClientExtensions
    {
        public static void AddAdapiClient(
            this IServiceCollection services,
            Func<IServiceProvider, IAdapiClientConfiguration> adapiClientConfigurationImplementationFactory)
        {
            services.AddScoped<IAdapiClient, AdapiClient>();
            services.AddHttpClient<IAdapiHttpClient, AdapiHttpClient>();
            services.AddScoped(adapiClientConfigurationImplementationFactory);
        }
    }
}
