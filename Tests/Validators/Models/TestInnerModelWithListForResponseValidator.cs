using MobileBff.Attributes;

namespace Tests.Validators.Models
{
    internal class TestInnerModelWithListForResponseValidator
    {
        [BffRequired]
        public List<TestInnerModelForResponseValidator> ObjectsListProperty { get; set; }
            = new List<TestInnerModelForResponseValidator> { new TestInnerModelForResponseValidator() };
    }
}
