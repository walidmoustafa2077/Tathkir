using Tathkīr_WPF.Enums;
using Tathkīr_WPF.Extensions;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services.CoreService.Interfaces;

namespace Tathkīr_WPF.Services.CoreService
{
    public class PrayerTimesService : IPrayerTimesService
    {
        private readonly HttpHostService _hostService;

        public PrayerTimesService()
        {
            _hostService = HttpHostService.Instance;
        }

        public async Task<PrayerTimesResult?> GetPrayerTimesAsync(DateTime date, bool use24HourFormat)
        {
            var result = await _hostService.GetTodayPrayerTimesAsync(date);
            if (result?.Data == null) return null;

            var timings = result.Data.Timings;
            var prayers = new List<PrayerItem>
        {
            new PrayerItem { Type = PrayerType.Fajr,    Time = timings.Fajr.FormatTime(use24HourFormat) },
            new PrayerItem { Type = PrayerType.Sunrise, Time = timings.Sunrise.FormatTime(use24HourFormat) },
            new PrayerItem { Type = PrayerType.Dhuhr,   Time = timings.Dhuhr.FormatTime(use24HourFormat) },
            new PrayerItem { Type = PrayerType.Asr,     Time = timings.Asr.FormatTime(use24HourFormat) },
            new PrayerItem { Type = PrayerType.Maghrib, Time = timings.Maghrib.FormatTime(use24HourFormat) },
            new PrayerItem { Type = PrayerType.Isha,    Time = timings.Isha.FormatTime(use24HourFormat) }
        };

            if (date.DayOfWeek == DayOfWeek.Friday)
            {
                var dhuhr = prayers.FirstOrDefault(p => p.Type == PrayerType.Dhuhr);
                if (dhuhr != null) dhuhr.Type = PrayerType.Jumua;
            }

            return new PrayerTimesResult
            {
                Prayers = prayers,
                Midnight = timings.Midnight.FormatTime(use24HourFormat),
                LastThird = timings.Lastthird.FormatTime(use24HourFormat),
                CurrentDate = result.Data.Date.FormatCurrentDate()
            };
        }
    }

}
