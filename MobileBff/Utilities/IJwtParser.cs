namespace MobileBff.Utilities
{
    public interface IJwtParser
    {
        string GetUserId(string? jwtToken);
    }
}