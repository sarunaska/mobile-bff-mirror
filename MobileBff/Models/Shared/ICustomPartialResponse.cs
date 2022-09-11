using System.Text.Json.Serialization;

namespace MobileBff.Models.Shared
{
    public interface ICustomPartialResponse
    {
        public bool IsPartialResponse { get; }
    }
}
