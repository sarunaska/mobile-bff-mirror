namespace A3SClient.HttpClients
{
    internal interface IA3SHttpClient
    {
        Task<T?> MakePostRequest<T>(Uri requestUri, string jwtAssertionToken, string content);
    }
}