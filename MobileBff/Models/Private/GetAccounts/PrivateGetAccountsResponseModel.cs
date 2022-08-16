using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccounts;

namespace MobileBff.Models.Private.GetAccounts
{
    public class PrivateGetAccountsResponseModel
    {
        [JsonPropertyName("retrieved_date_time")]

        public DateTime RetrievedDateTime { get; }

        [JsonPropertyName("account_groups")]
        public PrivateAccountGroupModel[]? AccountGroups { get; }

        public PrivateGetAccountsResponseModel(string userId, Dictionary<string, GetAccountsResult> results)
        {
            if (!results.Any())
            {
                RetrievedDateTime = DateTime.Now;
                return;
            }

            RetrievedDateTime = results.First().Value.RetrievedDateTime;

            AccountGroups = results.Select(x => new PrivateAccountGroupModel(x.Key, x.Value.Accounts, x.Key != userId)).ToArray();
        }
    }
}