using A3SClient.Models;
using MobileBff.Models;

namespace Tests.ResponseProviders
{
    internal static class A3SResponseProvider
    {
        public static GetUserCustomersResponse CreateGetUserCustomersResponse(
            bool returnNoUserCustomers = false,
            bool addPowerOfAttorneyAccount = false)
        {
            var userCustomers = returnNoUserCustomers
                ? new List<UserCustomer>()
                : new List<UserCustomer>
                    {
                        new UserCustomer
                        {
                            PublicIdentifier = "Test User Id",
                            AuthorizationScope = new[] { Constants.AuthorizationScopes.Private, "Test AuthorizationScope" }
                        },
                        new UserCustomer
                        {
                            PublicIdentifier = "Test PublicIdentifier",
                            AuthorizationScope = new[] { "Test AuthorizationScope" }
                        }
                    };

            if (addPowerOfAttorneyAccount)
            {
                userCustomers.Add(
                    new UserCustomer
                    {
                        PublicIdentifier = "Test PowerOfAttorney UserId",
                        AuthorizationScope = new[] { Constants.AuthorizationScopes.Private, "Test AuthorizationScope" }
                    });
            }

            return new GetUserCustomersResponse
            {
                Data = new Data
                {
                    UserCustomers = userCustomers.ToArray()
                }
            };
        }
    }
}
