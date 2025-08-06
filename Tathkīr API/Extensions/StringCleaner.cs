namespace Tathkīr_API.Extensions
{
    public static class StringCleaner
    {
        public static string RemoveKeywords(this string input, IEnumerable<string> keywords)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            foreach (var keyword in keywords)
            {
                input = input.Replace(keyword, "", StringComparison.OrdinalIgnoreCase);
            }

            return input.Trim();
        }
    }

}
