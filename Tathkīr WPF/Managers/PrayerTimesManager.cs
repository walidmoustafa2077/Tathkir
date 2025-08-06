using System.Net;
using System.Windows.Threading;
using Tathkīr_WPF.Helpers;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services;

namespace Tathkīr_WPF.Managers
{
    public class PrayerTimesManager
    {
        private readonly HostService _prayerTimesService;
        private static PrayerTimesManager? _instance;
        public static PrayerTimesManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PrayerTimesManager();
                }
                return _instance;
            }
        }
        private static IClock _clock = new SystemClock();
        public PrayerTimesResult? PrayerTimesResult { get; private set; }

        private List<PrayerItem> _prayerItems => PrayerTimesResult?.Prayers ?? new List<PrayerItem>();

        private DateTime _loadedDate = DateTime.Today;
        private PrayerItem? _nextPrayer;
        private DateTime? _nextPrayerTime;

        private readonly DispatcherTimer _timer;

        public PrayerItem? NextPrayer => _nextPrayer;
        public DateTime? NextPrayerTime => _nextPrayerTime;
        public TimeSpan Countdown => _nextPrayerTime.HasValue ? _nextPrayerTime.Value - _clock.Now : TimeSpan.Zero;


        public event Action? CountdownUpdated;
        public event Action? NewPrayerCycleStarted;

        public PrayerTimesManager()
        {
            _prayerTimesService = HostService.Instance;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (s, e) => Timer_Tick();
        }

        public static void SetClockForTests(IClock clock)
        {
            _clock = clock;
        }

        public async Task LoadPrayerTimesAsync(bool use24HourFormat = false)
        {
            PrayerTimesResult = await BuildPrayerTimesAsync(_loadedDate, use24HourFormat);

            if (PrayerTimesResult != null)
            {
                _timer.Start();
                UpdateNextPrayer();
            }
        }

        public async Task<PrayerTimesResult> LoadPrayerTimesPerDayAsync(DateTime date, bool use24HourFormat = false)
        {
            return await BuildPrayerTimesAsync(date, use24HourFormat) ?? new PrayerTimesResult();
        }

        private async Task<PrayerTimesResult?> BuildPrayerTimesAsync(DateTime date, bool use24HourFormat)
        {
            var result = await _prayerTimesService.GetTodayPrayerTimesAsync(date);
            if (result?.Data == null)
                return null;

            var timings = result.Data.Timings;
            var prayers = new List<PrayerItem>
            {
                new PrayerItem { Type = Enums.PrayerType.Fajr,    Time = FormatTime(timings.Fajr, use24HourFormat) },
                new PrayerItem { Type = Enums.PrayerType.Sunrise, Time = FormatTime(timings.Sunrise, use24HourFormat) },
                new PrayerItem { Type = Enums.PrayerType.Dhuhr,   Time = FormatTime(timings.Dhuhr, use24HourFormat) },
                new PrayerItem { Type = Enums.PrayerType.Asr,     Time = FormatTime(timings.Asr, use24HourFormat) },
                new PrayerItem { Type = Enums.PrayerType.Maghrib, Time = FormatTime(timings.Maghrib, use24HourFormat) },
                new PrayerItem { Type = Enums.PrayerType.Isha,    Time = FormatTime(timings.Isha, use24HourFormat) }
            };

            if (date.DayOfWeek == DayOfWeek.Friday)
            {
                var dhuhr = prayers.FirstOrDefault(p => p.Type == Enums.PrayerType.Dhuhr);
                if (dhuhr != null)
                    dhuhr.Type = Enums.PrayerType.Jumua;
            }

            return new PrayerTimesResult
            {
                Prayers = prayers,
                Midnight = FormatTime(timings.Midnight, use24HourFormat),
                LastThird = FormatTime(timings.Lastthird, use24HourFormat),
                CurrentDate = FormatCurrentDate(result.Data.Date)
            };
        }

        private void Timer_Tick()
        {
            if (DateTime.Today > _loadedDate)
            {
                _ = ReloadAsync();
                return;
            }

            if (_nextPrayerTime.HasValue && _clock.Now >= _nextPrayerTime.Value)
            {
                UpdateNextPrayer();
            }

            CountdownUpdated?.Invoke();
        }

        private void UpdateNextPrayer()
        {
            SetNextPrayer();
            NewPrayerCycleStarted?.Invoke();
        }

        private void SetNextPrayer()
        {
            if (PrayerTimesResult == null || PrayerTimesResult.Prayers.Count == 0)
            {
                _nextPrayer = null;
                _nextPrayerTime = null;
                return;
            }

            var now = _clock.Now;

            foreach (var prayer in PrayerTimesResult.Prayers)
            {
                if (DateTime.TryParse(prayer.Time, out var prayerTime))
                {
                    var fullTime = new DateTime(now.Year, now.Month, now.Day, prayerTime.Hour, prayerTime.Minute, 0);
                    if (fullTime > now)
                    {
                        _nextPrayer = prayer;
                        _nextPrayerTime = fullTime;
                        _nextPrayer.IsNextPrayer = true;
                        return;
                    }
                }
            }

            if (_prayerItems.Count > 0 && DateTime.TryParse(_prayerItems[0].Time, out var firstTime))
            {
                _nextPrayer = _prayerItems[0];
                _nextPrayerTime = new DateTime(now.Year, now.Month, now.Day, firstTime.Hour, firstTime.Minute, 0).AddDays(1);
                _nextPrayer.IsNextPrayer = true;
            }
            else
            {
                _nextPrayer = null;
                _nextPrayerTime = null;
            }
        }

        private async Task ReloadAsync()
        {
            _timer.Stop();
            _loadedDate = DateTime.Today;
            await LoadPrayerTimesAsync();
        }

        private string FormatTime(string? time, bool use24HourFormat)
        {
            if (string.IsNullOrWhiteSpace(time))
                return string.Empty;

            if (DateTime.TryParseExact(time, "HH:mm", null, System.Globalization.DateTimeStyles.None, out var parsedTime))
                return use24HourFormat ? parsedTime.ToString("HH:mm") : parsedTime.ToString("hh:mm tt");

            return time ?? string.Empty;
        }

        private string FormatCurrentDate(Models.PrayerDate date)
        {
            if (DateTime.TryParse(date.Readable, out var gregorianDate))
            {
                var weekday = gregorianDate.ToString("dddd", System.Globalization.CultureInfo.InvariantCulture);
                var hijriDay = int.TryParse(date.Hijri?.Date.Split('-')[0], out var day)
                    ? day.ToString()
                    : string.Empty;
                var hijriMonth = date.Hijri?.Month?.En ?? string.Empty;
                var gregorianShort = gregorianDate.ToString("dd MMM", System.Globalization.CultureInfo.InvariantCulture);

                return $"{weekday} {hijriDay} {hijriMonth} · {gregorianShort}";
            }

            return date.Readable ?? string.Empty;
        }
    }

}
