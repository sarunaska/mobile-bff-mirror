using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet;
using Tests.API.Contracts;
using Tests.ResponseProviders;
using Xunit.Abstractions;
using Match = PactNet.Matchers.Match;
using Constants = MobileBff.Models.Constants;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Tests.API.PrivateAccounts
{
    [Collection("API Test Collection")]
    public class GetAccounts : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly HttpClient _client;
        private readonly IPactBuilderV3 _pactBuilder;
        private readonly int _pactMockPort = 9090;
            
        public GetAccounts(WebApplicationFactory<Program> factory, ITestOutputHelper output, ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = output;
            factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, configBuilder) =>
                {
                    configBuilder.AddInMemoryCollection(
                        new Dictionary<string, string>
                        {
                            ["Adapi:Url"] = $"http://localhost:{_pactMockPort}"
                        });
                });
            });
            
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Add("organization-id", "test-organizationId");
            _client.DefaultRequestHeaders.Add("jwt-Assertion", "test-jwtAssertion");
            
            // Use default pact directory ..\..\pacts and default log
            // directory ..\..\logs
            var pact = Pact.V3("Mobile app BFF", "ADAPI", new PactConfig
            {
                PactDir = @"..\..\..\..\Tests.Pacts\",
                DefaultJsonSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                },
                Outputters = new[]
                {
                    new XUnitOutput(output)
                },
            });
            // Initialize Rust backend
            _pactBuilder = pact.UsingNativeBackend(_pactMockPort);
        }

        [Fact]
        public async Task WhenGetAccounts_ThenAccountListReturned()
        {
            _pactBuilder
                .UponReceiving("A GET request to get all accounts")
                .Given("There is a single account with SEK currency")
                    .WithHeader("organization-id", "test-organizationId")
                    .WithHeader("jwt-Assertion", "test-jwtAssertion")
                    .WithRequest(HttpMethod.Get, "/accounts")
                .WillRespond()
                    .WithStatus(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                .WithJsonBody(
                    Match.Type(AdapiResponseProvider.CreateGetAccountsResponse(currency: Constants.Currencies.SEK)));

            await _pactBuilder.VerifyAsync(async _ =>
            {
                var accountsResponse = await _client.GetFromJsonAsync<AccountsResult>("/corp/accounts");
                _testOutputHelper.WriteLine(JsonSerializer.Serialize(accountsResponse));
                Assert.NotNull(accountsResponse);
                Assert.NotEmpty(accountsResponse!.AccountGroups);
                Assert.NotEmpty(accountsResponse.AccountGroups.ToList()[0].Accounts);
            });
        }
    }
}