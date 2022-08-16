using MobileBff.Formatters;

namespace Tests.Formatters
{
    public class IbanFormatterTests
    {
        [Fact]
        public void Format_WhenIbanContains24Characters_ShouldReturnFormattedIban()
        {
            var formatedIban = IbanFormatter.Format("SE4850000000052800046651");

            Assert.Equal("SE48 5000 0000 0528 0004 6651", formatedIban);
        }

        [Fact]
        public void Format_WhenIbanContains27Characters_ShouldReturnFormattedIban()
        {
            var formatedIban = IbanFormatter.Format("FR7630006000011234567890189");

            Assert.Equal("FR76 3000 6000 0112 3456 7890 189", formatedIban);
        }

        [Fact]
        public void Format_WhenIbanIsEmptyString_ShouldReturnEmptyString()
        {
            var formatedIban = IbanFormatter.Format(string.Empty);

            Assert.Equal(string.Empty, formatedIban);
        }
    }
}
