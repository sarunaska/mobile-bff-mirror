using System.Net;

namespace MobileBff.Services
{
    public class PingAdapiService : IPingAdapiService
    {
        private readonly HttpClient httpClient;

        public PingAdapiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> Ping()
        {
            var response = await httpClient.GetAsync("/ping");
            return response.StatusCode == HttpStatusCode.NoContent;
        }
    }
}
