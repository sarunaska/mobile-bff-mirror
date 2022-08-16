using System.Collections.Specialized;
using System.Web;
using AdapiClient.Constants;

namespace AdapiClient.Builders
{
    internal class AdapiUriBuilder
    {
        private readonly Uri baseUri;
        private readonly string relativeUri;
        private NameValueCollection? parameters;

        public AdapiUriBuilder(Uri baseUri, string relativeUri)
        {
            this.baseUri = baseUri;
            this.relativeUri = relativeUri;
        }

        public AdapiUriBuilder AddQueryParameter(string name, string? value)
        {
            if (value == null)
            {
                return this;
            }

            if (parameters == null)
            {
                parameters = HttpUtility.ParseQueryString(string.Empty);
            }

            parameters.Add(name, value);

            return this;
        }

        public Uri Build()
        {
            var uriBuilder = new UriBuilder(new Uri(baseUri, relativeUri))
            {
                Query = parameters?.ToString() ?? string.Empty
            };

            return uriBuilder.Uri;
        }

        public AdapiUriBuilder AddPagingParameters(string? key, string? size)
        {
            AddQueryParameter(AdapiUrlQueryParameters.PaginatingKey, key);
            AddQueryParameter(AdapiUrlQueryParameters.PaginatingSize, size);

            return this;
        }
    }
}
