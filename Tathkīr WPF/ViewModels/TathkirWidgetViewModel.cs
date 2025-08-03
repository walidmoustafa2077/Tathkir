using System.Collections.ObjectModel;
using System.IO;
using Tathkīr_WPF.Helpers;
using Tathkīr_WPF.Managers;
using Tathkīr_WPF.Models;

namespace Tathkīr_WPF.ViewModels
{
    public class TathkirWidgetViewModel : ViewModelBase
    {
        private readonly PrayerTimesManager _manager;
        private Audios? _audios;
        private string _cachedNextPrayerName = string.Empty;

        public ObservableCollection<PrayerItem> PrayerList { get; } = new();

        private string _nextPrayerIcon = string.Empty;
        public string NextPrayerIcon { get => _nextPrayerIcon; set { _nextPrayerIcon = value; OnPropertyChanged(); } }

        private string _nextPrayerName = string.Empty;
        public string NextPrayerName { get => _nextPrayerName; set { _nextPrayerName = value; OnPropertyChanged(); } }

        private string _countdown = string.Empty;
        public string Countdown { get => _countdown; set { _countdown = value; OnPropertyChanged(); } }

        private string _midnightTime = string.Empty;
        public string MidnightTime { get => _midnightTime; set { _midnightTime = value; OnPropertyChanged(); } }

        private string _lastThirdTime = string.Empty;
        public string LastThirdTime { get => _lastThirdTime; set { _lastThirdTime = value; OnPropertyChanged(); } }

        private string _currentDate = string.Empty;
        public string CurrentDate { get => _currentDate; set { _currentDate = value; OnPropertyChanged(); } }

        public bool Use24HourFormat { get; set; } = false;

        public TathkirWidgetViewModel()
        {
            _manager = PrayerTimesManager.Instance;
            //var testClock = new TestClock();
            //testClock.Advance(TimeSpan.FromMinutes(-144)); // Advance by 1 minute to simulate countdown

            //PrayerTimesManager.SetClockForTests(testClock);
            //_manager.LoadPrayerTimesAsync(_manager.Address, Use24HourFormat);
            Initialize();
        }

        private void Initialize()
        {
            if (_manager.PrayerTimesResult == null)
                return;

            PrayerList.Clear();
            foreach (var p in _manager.PrayerTimesResult.Prayers)
                PrayerList.Add(p);

            MidnightTime = _manager.PrayerTimesResult.Midnight;
            LastThirdTime = _manager.PrayerTimesResult.LastThird;
            CurrentDate = _manager.PrayerTimesResult.CurrentDate;

            _manager.CountdownUpdated += OnCountdownUpdated;
            _manager.NewPrayerCycleStarted += OnNewPrayerStarted;

            UpdateNextPrayerUI();
            UpdateCountdown();

        }

        private void OnNewPrayerStarted()
        {
            UpdateNextPrayerUI();
        }

        private void OnCountdownUpdated()
        {
            UpdateCountdown();
        }

        private void UpdateNextPrayerUI()
        {
            var next = _manager.NextPrayer;

            if (next != null)
            {
                NextPrayerName = $"{Strings.Time_Till} {next.Name}";
                NextPrayerIcon = next.Icon;
            }
            else
            {
                NextPrayerName = "No upcoming prayer";
                NextPrayerIcon = "ClockOutline";
            }
        }

        private void UpdateCountdown()
        {
            var countdown = _manager.Countdown;

            if (_manager.NextPrayer != null && countdown <= TimeSpan.FromMinutes(5) && _cachedNextPrayerName != NextPrayerName)
            {
                PlayAudio("BeforeAthanPath");
                _cachedNextPrayerName = NextPrayerName;
            }

            if (countdown <= TimeSpan.FromSeconds(1) && _cachedNextPrayerName == NextPrayerName)
            {
                PlayAudio("OnAthanPath");
                _cachedNextPrayerName = string.Empty;
            }

            Countdown = $"{(int)countdown.TotalHours:D2}:{countdown.Minutes:D2}:{countdown.Seconds:D2}";
        }

        private void PlayAudio(string timing)
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Audios.json");
            if (File.Exists(jsonPath))
            {
                var json = File.ReadAllText(jsonPath);
                _audios = System.Text.Json.JsonSerializer.Deserialize<Audios>(json);
            }

            var prayerName = _manager.NextPrayer?.Type.ToString();

            var audioEntry = _audios?.PrayAudios
                .FirstOrDefault(a => a.Name.Equals(prayerName, StringComparison.OrdinalIgnoreCase));

            string? audioFilePath = null;

            if (audioEntry != null)
            {
                var prop = audioEntry.GetType().GetProperty(timing);
                audioFilePath = prop?.GetValue(audioEntry) as string;
            }



            if (!string.IsNullOrWhiteSpace(audioFilePath))
            {
                audioFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Audios", audioFilePath);

                if (audioFilePath.Contains("before", StringComparison.OrdinalIgnoreCase))
                {
                    // Do something if "before" exists in the string
                    ToastHelper.ShowToast("Prayer Reminder", $"{Strings.The_Time_For} {_manager.NextPrayer?.Name} {Strings.Prayer_Is_Approaching}", audioFilePath);

                    return;
                }

                ToastHelper.ShowToast("Prayer Reminder", $"{Strings.It_s_Time_For} {_manager.NextPrayer?.Name} {Strings.Prayer}", audioFilePath);
            }
        }

    }

}