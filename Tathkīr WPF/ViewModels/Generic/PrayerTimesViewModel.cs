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

        public Brush SeparatorBackground
        {
            get
            {
                var theme = new PaletteHelper().GetTheme().GetBaseTheme();

                var resourceKey = theme == BaseTheme.Light
                    ? "MaterialDesignDarkSeparatorBackground"
                    : "MaterialDesignLightSeparatorBackground";

                return Application.Current.Resources[resourceKey] as Brush ?? Brushes.Transparent;
            }
        }

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

            _manager.NewPrayerCycleStarted += () =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    PrayerList.Clear();
                    foreach (var p in _manager.PrayerTimesResult!.Prayers)
                        PrayerList.Add(p);
                });
            };
        }

        private void NextDate(object? obj)
        {
            var date = CurrentDate.ConvertDate();
            date = date.AddDays(Application.Current.MainWindow.FlowDirection == FlowDirection.RightToLeft ? -1 : 1);

            if (ResetDateVisibility == false)
                ResetDateVisibility = true;

            var result = _manager.GetPrayerTimeAsync(date: date);

            result.ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        PrayerList.Clear();
                        foreach (var p in task.Result!.Prayers)
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
            var date = CurrentDate.ConvertDate();
            date = date.AddDays(Application.Current.MainWindow.FlowDirection == FlowDirection.RightToLeft ? 1 : -1);

            if (ResetDateVisibility == false)
                ResetDateVisibility = true;

            var result = _manager.GetPrayerTimeAsync(date: date);

            result.ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        PrayerList.Clear();
                        foreach (var p in task.Result!.Prayers)
                            PrayerList.Add(p);

                        MidnightTime = task.Result.Midnight;
                        LastThirdTime = task.Result.LastThird;
                        CurrentDate = task.Result.CurrentDate;
                    });
                }
            });
        }
    }
}
