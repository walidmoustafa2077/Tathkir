using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using Tathkīr_WPF.Commands;
using Tathkīr_WPF.Managers;
using Tathkīr_WPF.Models;

namespace Tathkīr_WPF.ViewModels.Generic
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly PrayerTimesManager _manager;

        private string _nextPrayerIcon = string.Empty;
        public string NextPrayerIcon { get => _nextPrayerIcon; set { _nextPrayerIcon = value; OnPropertyChanged(); } }

        private string _nextPrayerName = string.Empty;
        public string NextPrayerName { get => _nextPrayerName; set { _nextPrayerName = value; OnPropertyChanged(); } }

        private string _nextPrayerTime = string.Empty;
        public string NextPrayerTime { get => _nextPrayerTime; set { _nextPrayerTime = value; OnPropertyChanged(); } }

        private string _countdown = string.Empty;
        public string Countdown { get => _countdown; set { _countdown = value; OnPropertyChanged(); } }

        private string _currentDate = string.Empty;
        public string CurrentDate { get => _currentDate; set { _currentDate = value; OnPropertyChanged(); } }
        public ObservableCollection<PrayerItem> PrayerList { get; } = new();
        public ObservableCollection<int> TasbeehLimits { get; set; } = new ObservableCollection<int> { 33, 99, 100 };

        // Tasbeeh properties
        private int _tasbeehCount;
        public int TasbeehCount
        {
            get => _tasbeehCount;
            set { _tasbeehCount = value; OnPropertyChanged(); }
        }

        private int _tasbeehLimit = 33; 
        public int TasbeehLimit
        {
            get => _tasbeehLimit;
            set { _tasbeehLimit = value; OnPropertyChanged(); }
        }

        private string _tasbeehTotal = "Total: 0";
        public string TasbeehTotal
        {
            get => _tasbeehTotal;
            set { _tasbeehTotal = value; OnPropertyChanged(); }
        }


        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<HijriEvent> HijriEvents { get; set; } = new ObservableCollection<HijriEvent>();

        public DateTime CurrentMonth => DateTime.Now;

        public ICommand IncrementTasbeehCommand { get; }

        public HomeViewModel(Views.GenericViews.HomeControl homeControl)
        {
            _manager = PrayerTimesManager.Instance;

            // Command to increment Tasbeeh count
            IncrementTasbeehCommand = new CommandBase((o) =>
            {
                TasbeehCount++;
                TasbeehTotal = $"{Strings.Total}: {TasbeehCount}";

                if (TasbeehCount >= TasbeehLimit)
                {
                    TasbeehCount = 0; // Reset after reaching limit
                }
            });

            // Subscribe to collection changed
            HijriEvents.CollectionChanged += (s, e) =>
            {
                // Refresh highlights on the UI thread
                Application.Current.Dispatcher.Invoke(() =>
                {
                    homeControl.HighlightEventDates();
                });
            };

            // Tasbeeh initial values
            TasbeehCount = 0;
            TasbeehTotal = $"{Strings.Total}: 0";

            Initialize();
        }

        private void Initialize()
        {
            if (_manager.PrayerTimesResult == null)
                return;

            PrayerList.Clear();

            foreach (var p in _manager.PrayerTimesResult.Prayers)
                PrayerList.Add(p);

            CurrentDate = _manager.PrayerTimesResult.CurrentDate;

            _manager.CountdownUpdated += UpdateCountdown;
            _manager.NewPrayerCycleStarted += UpdateNextPrayerUI;

            UpdateNextPrayerUI();
            UpdateCountdown();

        }

        private void UpdateNextPrayerUI()
        {
            var next = _manager.NextPrayer;

            if (next != null)
            {
                NextPrayerName = $"{Strings.Time_Till} {next.Name}";
                NextPrayerTime = next.Time;
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
