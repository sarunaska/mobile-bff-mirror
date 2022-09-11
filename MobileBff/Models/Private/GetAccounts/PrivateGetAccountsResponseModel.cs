using System.Text.Json.Serialization;
using AdapiClient.Endpoints.GetAccounts;
using MobileBff.Attributes;
using SebCsClient.Models;

namespace MobileBff.Models.Private.GetAccounts
{
    public class PrivateGetAccountsResponseModel : IPartialResponseModel
    {
        [JsonPropertyName("retrieved_date_time")]

        public DateTime RetrievedDateTime { get; }

        [BffRequired]
        [JsonPropertyName("account_groups")]
        public List<PrivateAccountGroupModel>? AccountGroups { get; }

        public PrivateGetAccountsResponseModel(
            List<(AccountOwner? AccountOwner, GetAccountsResult Result)> userAccounts)
        {
            if (!userAccounts.Any())
            {
                RetrievedDateTime = DateTime.UtcNow;
                return;
            }

            RetrievedDateTime = userAccounts.First().Result.RetrievedDateTime ?? DateTime.UtcNow;

            AccountGroups = userAccounts
                .Select(x => new PrivateAccountGroupModel(
                    x.AccountOwner,
                    x.Result.Accounts))
                .OrderBy(x => x.Owner?.Name)
                .ToList();
        }
    }
}