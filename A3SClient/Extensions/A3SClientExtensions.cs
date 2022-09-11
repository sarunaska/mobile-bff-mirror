using A3SClient.Configuration;
using A3SClient.HttpClients;
using Microsoft.Extensions.DependencyInjection;

namespace A3SClient.Extensions
{
    public static class A3SClientExtensions
    {
        public static void AddA3SClient(
            this IServiceCollection services,
            Func<IServiceProvider, IA3SClientConfiguration> a3sClientConfigurationImplementationFactory)
        {
            services.AddScoped<IA3SClient, A3SClient>();
            services.AddHttpClient<IA3SHttpClient, A3SHttpClient>();
            services.AddScoped(a3sClientConfigurationImplementationFactory);
        }
    }
}
