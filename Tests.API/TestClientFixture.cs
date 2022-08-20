using Microsoft.Extensions.Hosting;

namespace Tests.API
{
    public class TestClientFixture : IAsyncLifetime
    {
        public HttpClient TestClient { get; set; }

        public TestClientFixture()
        {
            var hostBuilder = new HostBuilder();
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        public Task DisposeAsync()
        {
            throw new NotImplementedException();
        }
    }
}