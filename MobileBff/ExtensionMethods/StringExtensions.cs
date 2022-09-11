using System.Globalization;

namespace MobileBff.ExtensionMethods
{
    public static class StringExtensions
    {
        private const string DecimalSeparatorPeriod = ".";

        public static decimal? ToBankingDecimal(this string? number)
        {
            if (number == null)
            {
                return null;
            }

            var numberFormatInfo = new NumberFormatInfo() { NumberDecimalSeparator = DecimalSeparatorPeriod };
            if (!decimal.TryParse(number, NumberStyles.Number, numberFormatInfo, out var parsedNumber))
            {
                return null;
            }

            var roundedNumber = Math.Round(parsedNumber, 2, MidpointRounding.ToEven);

            return roundedNumber;
        }
    }
}
