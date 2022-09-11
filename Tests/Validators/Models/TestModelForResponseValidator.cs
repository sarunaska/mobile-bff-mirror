using MobileBff.Attributes;
using MobileBff.Models.Shared;

namespace Tests.Validators.Models
{
    internal class TestModelForResponseValidator : ICustomPartialResponse
    {
        [BffRequired]
        public string? RequiredStringProperty { get; set; } = "Test";

        public string? NonRequiredStringProperty { get; set; } = "Test";

        [BffRequired]
        public List<string>? RequiredStringsListProperty { get; set; } = new List<string> { "Test" };

        public List<string>? NonRequiredStringsListProperty { get; set; } = new List<string> { "Test" };

        [BffRequired]
        public TestInnerModelForResponseValidator? RequiredObjectProperty { get; set; } = new();

        public TestInnerModelForResponseValidator? NonRequiredObjectProperty { get; set; } = new();

        public List<TestInnerModelForResponseValidator> ObjectsListProperty { get; set; }
            = new List<TestInnerModelForResponseValidator> { new TestInnerModelForResponseValidator() };

        public List<TestInnerModelWithListForResponseValidator> ObjectsListWithInnerListProperty { get; set; }
            = new List<TestInnerModelWithListForResponseValidator> { new TestInnerModelWithListForResponseValidator() };

        public bool IsPartialResponse { get; set; }
    }
}
