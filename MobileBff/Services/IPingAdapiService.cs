namespace MobileBff.Services
{
    public interface IPingAdapiService
    {
        Task<bool> Ping();
    }
}