using MobileBff.Formatters;

namespace Tests.Formatters
{
    public class SwissValueFormatterTests
    {
        [Fact]
        public void Format_WhenNameIsNull_ShouldReturnFormattedId()
        {
            var formattedSwissValue = SwissValueFormatter.Format("0701234567", null);

            Assert.Equal("070 123 45 67", formattedSwissValue);
        }

        [Fact]
        public void Format_WhenNameIsNotNull_ShouldReturnFormattedIdWithName()
        {
            var formattedSwissValue = SwissValueFormatter.Format("0701234567", "Test Name");

            Assert.Equal("070 123 45 67 - Test Name", formattedSwissValue);
        }
    }
}
