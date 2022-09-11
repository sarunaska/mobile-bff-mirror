using A3SClient.Configuration;
using A3SClient.HttpClients;
using A3SClient.Models;
using Newtonsoft.Json;

namespace A3SClient
{
    internal class A3SClient : IA3SClient
    {
        private readonly IA3SHttpClient a3sHttpClient;
        private readonly IA3SClientConfiguration configuration;

        public A3SClient(IA3SHttpClient a3sHttpClient, IA3SClientConfiguration configuration)
        {
            this.configuration = configuration;
            this.a3sHttpClient = a3sHttpClient;
        }

        public async Task<GetUserCustomersResponse?> GetUserCustomers(
            string userId,
            string jwtAssertion)
        {
            string query = @"
                query($userId: String!)
                {
                    user_customers(user_id:$userId)
                    { 
                        public_identifier
                        authorization_scope
                    } 
                }";

            var graphQlQuery = new
            {
                query = query,
                variables = new
                {
                    userId = userId
                }
            };

            var serializedQuery = JsonConvert.SerializeObject(graphQlQuery);

            var requestUri = new Uri(configuration.BaseUrl, "graph");

            return await a3sHttpClient.MakePostRequest<GetUserCustomersResponse>(requestUri, jwtAssertion, serializedQuery);
        }
    }
}
