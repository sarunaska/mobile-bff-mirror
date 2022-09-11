using System.Text.Json.Serialization;

namespace MobileBff.Models.Errors
{
    public class UnhandledErrorModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; }

        [JsonPropertyName("type")]
        public string? Type { get; }

        [JsonPropertyName("message")]
        public string? Message { get; }

        [JsonPropertyName("stack_trace")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? StackTrace { get; }

        public UnhandledErrorModel(Exception ex, bool addDevelopmentDetails)
        {
            Id = Guid.NewGuid();
            Type = ex.GetType().Name;
            Message = ex.Message;
            StackTrace = addDevelopmentDetails ? ex.StackTrace : null;
        }
    }
}
