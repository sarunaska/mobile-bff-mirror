using System.IdentityModel.Tokens.Jwt;

namespace MobileBff.Utilities
{
    public class JwtParser : IJwtParser
    {
        private const string PreferredUsername = "preferred_username";
        private const string ErrorMessage = $"Failed to get user ID from JWT token";

        public string GetUserId(string? jwtToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(jwtToken);

                var userId = (string)jwtSecurityToken.Payload[PreferredUsername];
                return userId;
            }
            catch (Exception ex)
            {
                Log($"{ErrorMessage}. Exception: {ex.Message}");

                throw new Exception(ErrorMessage, ex);
            }
        }

        private static void Log(string message)
        {
            Console.Error.WriteLine(message);
        }
    }
}
