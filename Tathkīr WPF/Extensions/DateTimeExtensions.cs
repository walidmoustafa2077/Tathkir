
using System.Globalization;

namespace Tathkīr_WPF.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly CultureInfo Culture = CultureInfo.DefaultThreadCurrentCulture ?? CultureInfo.CurrentCulture;

        public static string FormatCurrentDate(this Models.PrayerDate date)
        {
            if (DateTime.TryParse(date.Readable, out var gregorianDate))
            {

                var weekday = gregorianDate.ToString("dddd", Culture);
                var hijriDay = int.TryParse(date.Hijri?.Date.Split('-')[0], out var day)
                    ? day.ToString()
                    : string.Empty;
                var month = date.Hijri?.Month;

                bool isRtl = Culture.TextInfo.IsRightToLeft;

                var hijriMonth = isRtl ? month?.Ar ?? string.Empty : month?.En ?? string.Empty;
                var gregorianShort = gregorianDate.ToString("dd MMM", Culture);

                return $"{weekday} {hijriDay} {hijriMonth} · {gregorianShort}";
            }

            return date.Readable ?? string.Empty;
        }


        public static string FormatTime(this string? time, bool use24HourFormat)
        {
            if (string.IsNullOrWhiteSpace(time))
                return string.Empty;

            if (DateTime.TryParseExact(time, "HH:mm", Culture, DateTimeStyles.None, out var parsedTime))
                return use24HourFormat
                    ? parsedTime.ToString("HH:mm", Culture)
                    : parsedTime.ToString("hh:mm tt", Culture);

            return time ?? string.Empty;
        }

        public static DateTime ConvertDate(this string formattedDate)
        {
            if (string.IsNullOrWhiteSpace(formattedDate))
                return DateTime.Now;

            // The format you used was:
            // $"{weekday} {hijriDay} {hijriMonth} · {gregorianShort}"
            // Example: "Monday 23 Muharram · 04 Feb"
            // then extract the Gregorian short date part.

            var parts = formattedDate.Split('·');
            if (parts.Length != 2)
                return DateTime.Now;

            var gregorianShort = parts[1].Trim();

            // gregorianShort is something like "04 Feb"
            // We need to attach the current year or infer it
            var currentYear = DateTime.Now.Year;
            var dateString = $"{gregorianShort} {currentYear}"; // e.g., "04 Feb 2025"

            if (DateTime.TryParseExact(
                    dateString,
                    "dd MMM yyyy",
                    Culture,
                    DateTimeStyles.None,
                    out var date))
            {
                return date;
            }

            return DateTime.Now;
        }
    }
}
