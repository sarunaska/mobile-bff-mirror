namespace A3SClient.Utilities
{
    // this class is copied from the common-lib of Team-Asgard:
    // https://github.sebank.se/Team-Asgard/common-lib/blob/main/TeamAsgard.Common/Utilities/SebCustomerNumberHelper.cs

    internal class SebCustomerNumberHelper
    {
        public static string ConvertToTrimmedSebCustomerNumber(string personalIdentityNumber)
        {
            if (string.IsNullOrWhiteSpace(personalIdentityNumber))
                throw new ArgumentException($"Invalid argument: {nameof(personalIdentityNumber)}");

            personalIdentityNumber = personalIdentityNumber.Trim();
            if (!long.TryParse(personalIdentityNumber, out _))
                return personalIdentityNumber;

            if (personalIdentityNumber.Length > 5 &&
                long.Parse(personalIdentityNumber.Substring(2, 2)) == 0 &&
                long.Parse(personalIdentityNumber.Substring(4, 2)) > 49)
            {
                return personalIdentityNumber.Substring(2) + "0003";
            }

            return personalIdentityNumber.Length switch
            {
                12 => personalIdentityNumber.Substring(0, 2) switch
                {
                    "19" => personalIdentityNumber.Substring(2) + "0009",
                    "20" => personalIdentityNumber.Substring(2) + "0000",
                    _ => personalIdentityNumber
                },
                14 => personalIdentityNumber,
                _ => personalIdentityNumber
            };
        }
    }
}
