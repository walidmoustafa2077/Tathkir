using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Tathkīr_WPF.Commands;
using Tathkīr_WPF.Managers;
using Tathkīr_WPF.Models;

namespace Tathkīr_WPF.ViewModels.Generic
{
    public class PrayerTimesViewModel : ViewModelBase
    {
        private readonly PrayerTimesManager _manager;
        public ObservableCollection<PrayerItem> PrayerList { get; } = new();

        private string _midnightTime = string.Empty;
        public string MidnightTime { get => _midnightTime; set { _midnightTime = value; OnPropertyChanged(); } }

        private string _lastThirdTime = string.Empty;
        public string LastThirdTime { get => _lastThirdTime; set { _lastThirdTime = value; OnPropertyChanged(); } }
        
        private string _currentDate = string.Empty;
        public string CurrentDate { get => _currentDate; set { _currentDate = value; OnPropertyChanged(); } }

        private bool _resetDateVisibility = false;
        public bool ResetDateVisibility { get => _resetDateVisibility; set { _resetDateVisibility = value; OnPropertyChanged(); } }

        // Commands
        public ICommand ResetDateCommand { get; }
        public ICommand PreviousDateCommand { get; }
        public ICommand NextDateCommand { get; }

        public PrayerTimesViewModel()
        {
            _manager = PrayerTimesManager.Instance;

            ResetDateVisibility = false;

            ResetDateCommand = new CommandBase(obj => { ResetDateVisibility = false; Initialize(); });
            NextDateCommand = new CommandBase(NextDate);
            PreviousDateCommand = new CommandBase(PreviousDate);

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
        }

        private void NextDate(object? obj)
        {
            var date = ConvertDate(CurrentDate);
            date = date.AddDays(Application.Current.MainWindow.FlowDirection == FlowDirection.RightToLeft ? -1 : 1);


            ResetDateVisibility = true;
            var result = _manager.LoadPrayerTimesPerDayAsync(date);

            result.ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        PrayerList.Clear();
                        foreach (var p in task.Result.Prayers)
                            PrayerList.Add(p);

                        MidnightTime = task.Result.Midnight;
                        LastThirdTime = task.Result.LastThird;
                        CurrentDate = task.Result.CurrentDate;
                    });
                }
            });
        }
        private void PreviousDate(object? obj)
        {
            var date = ConvertDate(CurrentDate);
            date = date.AddDays(Application.Current.MainWindow.FlowDirection == FlowDirection.RightToLeft ? 1 : -1);

            ResetDateVisibility = true;
            var result = _manager.LoadPrayerTimesPerDayAsync(date);

            result.ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        PrayerList.Clear();
                        foreach (var p in task.Result.Prayers)
                            PrayerList.Add(p);

                        MidnightTime = task.Result.Midnight;
                        LastThirdTime = task.Result.LastThird;
                        CurrentDate = task.Result.CurrentDate;
                    });
                }
            });
        }

        private DateTime ConvertDate(string formattedDate)
        {
            if (string.IsNullOrWhiteSpace(formattedDate))
                return DateTime.Now;

            // The format you used was:
            // $"{weekday} {hijriDay} {hijriMonth} · {gregorianShort}"
            // Example: "Monday 23 Muharram · 04 Feb"
            // We need to extract the Gregorian short date part.

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
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out var date))
            {
                return date;
            }

            return DateTime.Now;
        }
    }
}
