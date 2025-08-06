using System.Text.RegularExpressions;

namespace Tathkīr_API.Extensions
{
    public static class StringExtensions
    {
        public static string NormalizeName(this string input)
        {
            return input
                .Replace("ā", "a")
                .Replace("á", "a")
                .Replace("ī", "i")
                .Replace("ū", "u")
                .Replace("ō", "o")
                .Replace("’", "'")
                .Replace("‘", "'")
                .Replace("ʿ", "'")
                .Replace("ḩ", "h")
                .Replace("Ḩ", "H")
                .Replace("ţ", "t")
                .Replace("Ţ", "T")
                .Replace("ḍ", "d")
                .Replace("ṣ", "s")
                .Replace("Ṣ", "S")
                .Replace("ḏ", "d")
                .Replace("ḥ", "h")
                .Replace("Ḥ", "H")
                .Replace("ḫ", "kh")
                .Replace("Ḳ", "Q")
                .Replace("ẓ", "z")
                .Replace("Ẓ", "Z")
                .Replace("ṯ", "th")
                .Replace("Ṯ", "Th");
        }

        public static string CapitalizeAlPrefix(this string name)
        {
            return Regex.Replace(name, @"\bal-", "Al-", RegexOptions.IgnoreCase);
        }

        public static bool ContainsLatinLetters(this string input)
        {
            return input.Any(c => c is >= 'A' and <= 'Z' or >= 'a' and <= 'z');
        }

        public static int LevenshteinDistance(this string s, string t)
        {
            if (string.IsNullOrEmpty(s)) return t.Length;
            if (string.IsNullOrEmpty(t)) return s.Length;

            var d = new int[s.Length + 1, t.Length + 1];

            for (int i = 0; i <= s.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= t.Length; j++) d[0, j] = j;

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = s[i - 1] == t[j - 1] ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[s.Length, t.Length];
        }


    }
}
