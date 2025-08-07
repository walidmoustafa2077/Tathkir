using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Tathkīr_WPF.Commands;
using Tathkīr_WPF.Extensions;
using Tathkīr_WPF.Managers;
using Tathkīr_WPF.Models;

namespace Tathkīr_WPF.ViewModels.Generic
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly PrayerTimesManager _manager;

        public ObservableCollection<PrayerItem> PrayerList { get; } = new();
        public ObservableCollection<int> TasbeehLimits { get; set; } = new ObservableCollection<int> { 33, 99, 100 };
        public ObservableCollection<HijriEvent> HijriEvents { get; set; } = new ObservableCollection<HijriEvent>();

        public Brush SeparatorBackground
        {
            get
            {
                var theme = new PaletteHelper().GetTheme().GetBaseTheme();

                var resourceKey = theme == BaseTheme.Dark
                    ? "MaterialDesignLightSeparatorBackground"
                    : "MaterialDesignDarkSeparatorBackground";

                return Application.Current.Resources[resourceKey] as Brush ?? Brushes.Transparent;
            }
        }

        public Brush CalendarHeaderBackground
        {
            get
            {
                var theme = new PaletteHelper().GetTheme().GetBaseTheme();

                if (theme == BaseTheme.Light)
                    return new SolidColorBrush(Color.FromRgb(11, 108, 157));
                else
                    return new SolidColorBrush((Color.FromRgb(11, 108, 157)).Lighten(0.6));
            }
        }

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

        // Tasbeeh properties
        private int _tasbeehCount;
        public int TasbeehCount
        {
            get => _tasbeehCount;
            set { _tasbeehCount = value; OnPropertyChanged(); }
        }

        private bool _resetTasbeehCountVisibility = false;
        public bool ResetTasbeehCountVisibility { get => _resetTasbeehCountVisibility; set { _resetTasbeehCountVisibility = value; OnPropertyChanged(); } }

        private int _tasbeehLimit = 33;
        public int TasbeehLimit
        {
            get => _tasbeehLimit;
            set { _tasbeehLimit = value; OnPropertyChanged(); }
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

        public DateTime CurrentMonth => DateTime.Now;

        public ICommand IncrementTasbeehCommand { get; }
        public ICommand ResetTasbeehCountCommand { get; }

        public HomeViewModel(Views.GenericViews.HomeControl homeControl)
        {
            _manager = PrayerTimesManager.Instance;

            // Command to increment Tasbeeh count
            IncrementTasbeehCommand = new CommandBase((o) =>
            {
                TasbeehCount++;

                if (!ResetTasbeehCountVisibility)
                    ResetTasbeehCountVisibility = true;

                if (TasbeehCount >= TasbeehLimit)
                {
                    TasbeehCount = 0;
                    ResetTasbeehCountVisibility = false;
                }
            });

            ResetTasbeehCountCommand = new CommandBase((o) =>
            {
                TasbeehCount = 0;
                ResetTasbeehCountVisibility = false;
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

            _manager.PrayerTimeUpdated += Initialize;
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
