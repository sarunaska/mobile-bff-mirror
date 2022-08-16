using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace A3SClient.HttpClients
{
    internal class A3SHttpClient : IA3SHttpClient
    {
        private const string JwtAssertionHeaderKey = "jwt-Assertion";

        private readonly HttpClient httpClient;

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        public A3SHttpClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<T> MakePostRequest<T>(Uri requestUri, string jwtAssertionToken, string content)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Headers = {
                    { JwtAssertionHeaderKey, jwtAssertionToken }
                },
                RequestUri = requestUri,
                Content = new StringContent(content),

            };
            var response = await httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T>(responseContent, jsonSerializerSettings);
        }
    }
}
