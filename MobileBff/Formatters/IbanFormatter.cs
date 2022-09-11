using System.Text.RegularExpressions;

namespace MobileBff.Formatters
{
    public static class IbanFormatter
    {
        public static string? Format(string? iban)
        {
            if (iban == null)
            {
                return null;
            }

            var formattedIban = Regex.Replace(iban, ".{4}", "$0 ").Trim();
            return formattedIban;
        }
    }
}
