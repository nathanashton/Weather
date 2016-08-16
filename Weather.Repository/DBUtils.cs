namespace Weather.Repository
{
    internal static class DbUtils
    {
        internal static int? ParseIntNull(string input)
        {
            int value;
            return int.TryParse(input, out value) ? (int?)value : null;
        }

        internal static int ParseIntZero(string input)
        {
            int value;
            return int.TryParse(input, out value) ? value : 0;
        }
    }
}