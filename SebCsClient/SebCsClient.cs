using SEB.SEBCS.RTM.v1.Client.Ckrm516.Kund.Hamta01.Grundinfo02;
using SebCsClient.Configuration;
using SebCsClient.Models;

namespace SebCsClient
{
    public class SebCsClient : ISebCsClient
    {
        private readonly HttpClient httpClient;

        public SebCsClient(HttpClient httpClient, ISebCsClientConfiguration sebCsClientConfiguration)
        {
            this.httpClient = httpClient;
            httpClient.BaseAddress = sebCsClientConfiguration.BaseUrl;
        }

        public async Task<AccountOwner?> GetAccountOwner(string userId, string jwtToken)
        {
            try
            {
                var client = new KundHamta01Grundinfo02Client(httpClient)
                {
                    RetrieveAuthorizationToken = () => Task.FromResult(jwtToken)
                };

                var result = await client.GetAsync(userId);

                return new AccountOwner
                {
                    Id = result.Result.Result.Kndv4d0.SebKundNr,
                    Name = result.Result.Result.Kndv4d0.Kundnamn
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
