namespace MobileBff.Models
{
    public static class Constants
    {
        public static class Actions
        {
            public const string Copy = "Copy";
            public const string ChangeAccountName = "ChangeAccountName";
        }

        public static class AliasTypes
        {
            public const string BankGiro = "bg";
            public const string Swish = "swish";
        }

        public static class AuthorizationScopes
        {
            public const string Private = "Private";
        }

        public static class BalanceTypes
        {
            public const string Booked = "booked";
            public const string Available = "available";
        }

        public static class Currencies
        {
            public const string SEK = "SEK";
        }

        public static class ProductCodes
        {
            public const string IskDepositAccounts = "1332";
            public const string Ips = "0102";
        }

        public static class ProductIds
        {
            public const string SwishForMerchants = "1548MBE";
            public const string SwishCorporate = "1522MBE";
        }

        public static class Titles
        {
            public const string Iban = "IBAN";
            public const string Bic = "BIC";
        }
    }
}
