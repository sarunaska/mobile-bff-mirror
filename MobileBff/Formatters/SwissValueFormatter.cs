namespace MobileBff.Formatters
{
    public static class SwissValueFormatter
    {
        public static string? Format(string? id, string? name)
        {
            if (id == null)
            {
                return null;
            }

            // format: 012 345 67 89
            var formattedId = $"{id.Substring(0, 3)} {id.Substring(3, 3)} {id.Substring(6, 2)} {id.Substring(8, 2)}";

            var formattedSwissValue = name == null
                ? formattedId
                : $"{formattedId} - {name}";

            return formattedSwissValue;
        }
    }
}
