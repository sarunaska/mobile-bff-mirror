namespace A3SClient.Exceptions
{
    public class A3SHttpClientException : Exception
    {
        public A3SHttpClientException(HttpResponseMessage httpResponseMessage) : base(GetErrorMessage(httpResponseMessage))
        {
        }

        private static string GetErrorMessage(HttpResponseMessage httpResponseMessage)
        {
            return $"Failed to send request to A3S. Response status code: {(int)httpResponseMessage.StatusCode} ({httpResponseMessage.StatusCode}). Request: {httpResponseMessage.RequestMessage?.Method} {httpResponseMessage.RequestMessage?.RequestUri}";
        }
    }
}
