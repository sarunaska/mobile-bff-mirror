using A3SClient.Models;

namespace A3SClient
{
    public interface IA3SClient
    {
        Task<GetUserCustomersResponse> GetUserCustomers(string userId, string jwtAssertion);
    }
}