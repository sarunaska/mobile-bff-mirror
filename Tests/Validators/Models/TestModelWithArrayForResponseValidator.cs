namespace Tests.Validators.Models
{
    internal class TestModelWithArrayForResponseValidator
    {
        public TestInnerModelForResponseValidator[] ObjectsArrayProperty { get; set; } = new[] { new TestInnerModelForResponseValidator() };
    }
}
