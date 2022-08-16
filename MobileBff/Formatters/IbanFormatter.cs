using System.Text.RegularExpressions;

namespace MobileBff.Formatters
{
    public static class IbanFormatter
    {
        public static string Format(string iban)
        {
            var formattedIban = Regex.Replace(iban, ".{4}", "$0 ").Trim();
            return formattedIban;
        }
    }
}
