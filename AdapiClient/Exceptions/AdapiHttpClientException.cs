namespace AdapiClient.Exceptions
{
    public class AdapiHttpClientException : Exception
    {
        public AdapiHttpClientException(HttpResponseMessage httpResponseMessage) : base(GetErrorMessage(httpResponseMessage))
        {
        }

        private static string GetErrorMessage(HttpResponseMessage httpResponseMessage)
        {
            return $"Failed to send request to ADAPI. Response status code: {(int)httpResponseMessage.StatusCode} ({httpResponseMessage.StatusCode}). Request: {httpResponseMessage.RequestMessage?.Method} {httpResponseMessage.RequestMessage?.RequestUri}";
        }
    }
}
