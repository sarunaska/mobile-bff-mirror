using AdapiClient.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AdapiClient.HttpClients
{
    internal class AdapiHttpClient : IAdapiHttpClient
    {
        private const string OrganizationHeaderKey = "organization-id";
        private const string JwtAssertionHeaderKey = "jwt-Assertion";
        private const string ClientIdHeaderKey = "client-id";

        private readonly HttpClient httpClient;

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        public AdapiHttpClient(HttpClient httpClient, IAdapiClientConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.httpClient.DefaultRequestHeaders.Add(ClientIdHeaderKey, configuration.ClientId);
        }

        public async Task<T> MakeGetRequest<T>(Uri requestUri, string organizationId, string jwtAssertionToken)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Headers =
                {
                    { OrganizationHeaderKey, organizationId },
                    { JwtAssertionHeaderKey, jwtAssertionToken }
                },
                RequestUri = requestUri
            };

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseBody, jsonSerializerSettings);
        }
    }
}
