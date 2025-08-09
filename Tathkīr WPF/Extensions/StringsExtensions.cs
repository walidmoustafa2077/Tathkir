using System.Globalization;
using System.Text;

namespace Tathkīr_WPF.Extensions
{
    public static class StringsExtensions
    {
        public static string NormalizeArabic(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = input.Replace("ـ", "");

            // Normalize into FormD (decomposed characters)
            string formD = input.Normalize(NormalizationForm.FormD);

            var sb = new StringBuilder();
            foreach (var ch in formD)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                // Skip diacritics (NonSpacingMark)
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }

            // Return recomposed text
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string GetIndicDigits(this int number)
        {
            var indicDigits = new[] { '٠', '١', '٢', '٣', '٤', '٥', '٦', '٧', '٨', '٩' };
            return string.Concat(number.ToString().Select(c => indicDigits[c - '0']));
        }
    }
}
