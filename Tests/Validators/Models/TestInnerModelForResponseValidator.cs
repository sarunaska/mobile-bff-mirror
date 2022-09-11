using MobileBff.Attributes;

namespace Tests.Validators.Models
{
    internal class TestInnerModelForResponseValidator
    {
        [BffRequired]
        public string? RequiredStringProperty { get; set; } = "Test";

        public string? NonRequiredStringProperty { get; set; } = "Test";
    }
}
