namespace AdapiClient.HttpClients
{
    internal interface IAdapiHttpClient
    {
        Task<T> MakeGetRequest<T>(Uri requestUri, string organizationId, string jwtAssertionToken);
    }
}