using System.Collections.ObjectModel;
using Tathkīr_WPF.Managers;
using Tathkīr_WPF.Models;

namespace Tathkīr_WPF.ViewModels
{
    public class TathkirWidgetViewModel : ViewModelBase
    {
        private readonly PrayerTimesManager _manager;

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

            _manager.PrayerTimeUpdated += Initialize;
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

            Countdown = $"{(int)countdown.TotalHours:D2}:{countdown.Minutes:D2}:{countdown.Seconds:D2}";
        }

    }

}