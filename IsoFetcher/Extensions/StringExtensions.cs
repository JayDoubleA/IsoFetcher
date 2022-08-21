namespace IsoFetcher.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        public static bool NotNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text) == false;
        }
    }
}
