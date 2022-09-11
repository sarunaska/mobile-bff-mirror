using SebCsClient.Models;

namespace SebCsClient
{
    public interface ISebCsClient
    {
        Task<AccountOwner?> GetAccountOwner(string userId, string jwtToken);
    }
}