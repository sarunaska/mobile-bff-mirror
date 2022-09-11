using System.Text.Json.Serialization;

namespace MobileBff.Models.Errors
{
    public class ValidationErrorModel
    {
        private const string ErrorMessage = "One or more validation errors occured.";

        [JsonPropertyName("id")]
        public Guid Id { get; }

        [JsonPropertyName("message")]
        public string? Message { get; }

        [JsonPropertyName("errors")]
        public List<string> Errors { get; }

        public ValidationErrorModel(string[] errorMessages)
        {
            Id = Guid.NewGuid();
            Message = ErrorMessage;
            Errors = new List<string>(errorMessages);
        }
    }
}
