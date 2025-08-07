using System.Windows.Threading;
using Tathkīr_WPF.Extensions;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services;
using Tathkīr_WPF.Services.CoreService;
using Tathkīr_WPF.Services.CoreService.Interfaces;

namespace Tathkīr_WPF.Managers
{
    public class PrayerTimesManager
    {
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

        private readonly IPrayerTimesService _prayerService;
        private readonly IPrayerScheduler _scheduler;
        private readonly IAudioService _audioService;
        private readonly INotificationService _notifier;
        private readonly IThikrReminderService _thikrService;
        private readonly DispatcherTimer _timer;
        private readonly IClock _clock;

        private PrayerTimesResult? _currentResult;
        public PrayerTimesResult? PrayerTimesResult => _currentResult;

        public PrayerItem? NextPrayer => _nextPrayer;
        public DateTime? NextPrayerTime => _nextPrayerTime;
        public TimeSpan Countdown => _nextPrayerTime.HasValue ? _nextPrayerTime.Value - _clock.Now : TimeSpan.Zero;

        private DateTime _loadedDate = DateTime.Today;
        private PrayerItem? _nextPrayer;
        private PrayerItem? _cachedNextPrayer;
        private DateTime? _nextPrayerTime;


        public event Action? PrayerTimeUpdated;
        public event Action? CountdownUpdated;
        public event Action? NewPrayerCycleStarted;

        public PrayerTimesManager()
        {
            var testClock = new TestClock();
            testClock.Advance(TimeSpan.FromMinutes(+220));

            _clock = testClock;
            _prayerService = new PrayerTimesService();
            _scheduler = new PrayerScheduler();
            _notifier = new NotificationService();
            _audioService = new AudioService();
            _thikrService = new ThikrReminderService(_clock, _notifier, _audioService);

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (_, _) => OnTick();
        }

        public async Task LoadPrayerTimesAsync(bool use24HourFormat = false)
        {
            _currentResult = await GetPrayerTimeAsync(use24HourFormat);

            if (_currentResult == null)
                return;

            UpdateNextPrayer();
            _timer.Start();
            PrayerTimeUpdated?.Invoke();
        }

        public async Task<PrayerTimesResult?> GetPrayerTimeAsync(bool use24HourFormat = false, DateTime? date = null)
        {
            return await _prayerService.GetPrayerTimesAsync(date ?? _loadedDate, use24HourFormat);
        }

        private void OnTick()
        {
            CountdownUpdated?.Invoke();

            // Check if the date has changed
            if (DateTime.Today > _loadedDate)
            {
                _ = ReloadAsync();
                return;
            }

            var now = _clock.Now;

            // Check if the next prayer time has passed
            if (_nextPrayerTime.HasValue && now >= _nextPrayerTime.Value)
            {
                UpdateNextPrayer();
            }



            var appConfig = SettingsService.AppSettings.AppConfig;

            if (appConfig.ShowNotifications == false)
                return;

            _thikrService.CheckThikrReminder();

            // Handle notifications
            if (_nextPrayer != null)
            {
                var timeToNext = _nextPrayerTime!.Value - now;
                var prayerName = _nextPrayer.Name;

                var selectedOffset = appConfig
                        .AthanConfigSettings?.BeforeAthanSettings
                        .FirstOrDefault(p => p.PrayerName.ToLocalizedLanguage() == prayerName)?.SelectedOffset;

                if (timeToNext.TotalMinutes <= selectedOffset && _nextPrayer == _cachedNextPrayer)
                {
                    _notifier.NotifyBeforePrayer(_nextPrayer, _audioService.GetPrayerAudioPath(_nextPrayer.Type.ToString(), "BeforeAthanPath"));
                    _cachedNextPrayer = null;
                }

                if (timeToNext.TotalSeconds <= 1)
                {
                    _notifier.NotifyOnPrayer(_nextPrayer, _audioService.GetPrayerAudioPath(_nextPrayer.Type.ToString(), "OnAthanPath"));
                }
            }
        }

        private void UpdateNextPrayer()
        {
            if (_currentResult == null)
                return;

            _nextPrayer = _scheduler.GetNextPrayer(_currentResult.Prayers, _clock.Now, out _nextPrayerTime);

            if (_nextPrayer != null)
                _cachedNextPrayer = _nextPrayer;

            NewPrayerCycleStarted?.Invoke();
        }

        private async Task ReloadAsync()
        {
            _timer.Stop();
            _loadedDate = DateTime.Today;
            await LoadPrayerTimesAsync();
        }
    }

}
