using System.Globalization;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services.CoreService.Interfaces;

namespace Tathkīr_WPF.Services.CoreService
{
    public class PrayerScheduler : IPrayerScheduler
    {
        public PrayerItem? GetNextPrayer(IEnumerable<PrayerItem> prayers, DateTime now, out DateTime? nextPrayerTime)
        {
            nextPrayerTime = null;

            foreach (var prayer in prayers)
            {
                if (DateTime.TryParseExact(prayer.Time.Trim(), "hh:mm tt",
                    CultureInfo.DefaultThreadCurrentCulture,
                    DateTimeStyles.None, out var time))
                {
                    var fullDateTime = new DateTime(now.Year, now.Month, now.Day, time.Hour, time.Minute, 0);
                    if (fullDateTime > now)
                    {
                        nextPrayerTime = fullDateTime;
                        prayer.IsNextPrayer = true;
                        return prayer;
                    }
                }
            }

            // fallback to first prayer on next day
            var firstPrayer = prayers.FirstOrDefault();
            if (firstPrayer != null && DateTime.TryParseExact(firstPrayer.Time.Trim(), "hh:mm tt",
                CultureInfo.DefaultThreadCurrentCulture,
                DateTimeStyles.None, out var firstTime))
            {
                nextPrayerTime = new DateTime(now.Year, now.Month, now.Day, firstTime.Hour, firstTime.Minute, 0).AddDays(1);
                firstPrayer.IsNextPrayer = true;
                return firstPrayer;
            }

            return null;
        }
    }

}
