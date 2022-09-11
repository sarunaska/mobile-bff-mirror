using Microsoft.Extensions.DependencyInjection;
using SebCsClient.Configuration;

namespace SebCsClient.Extensions
{
    public static class SebCsClientExtensions
    {
        public static void AddSebCsClient(
            this IServiceCollection services,
            Func<IServiceProvider, ISebCsClientConfiguration> sebCsClientConfigurationImplementationFactory)
        {
            services.AddScoped<ISebCsClient, SebCsClient>();
            services.AddScoped(sebCsClientConfigurationImplementationFactory);
        }
    }
}
