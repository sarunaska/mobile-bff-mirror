using MobileBff.ExtensionMethods;

namespace Tests.ExtensionMethods
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("-0.010", -0.01)]
        [InlineData("0.0", 0)]
        [InlineData("0.010", 0.01)]
        [InlineData("0.014", 0.01)]
        [InlineData("0.015", 0.02)]
        [InlineData("0.025", 0.02)]
        [InlineData("0.026", 0.03)]
        public void ToBankingDecimal_WhenDecimalNumberIsProvided_ShouldRoundItUsingHalfEvenRoundingRules(string stringDecimalNumber, decimal expectedDecimalNumber)
        {
            var parsedDecimal = stringDecimalNumber.ToBankingDecimal();

            Assert.Equal(expectedDecimalNumber, parsedDecimal);
        }
    }
}