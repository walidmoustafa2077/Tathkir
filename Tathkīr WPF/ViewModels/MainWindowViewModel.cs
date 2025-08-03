using System.Windows.Controls;
using Tathkīr_WPF.Views.GenericViews;
using Tathkīr_WPF.Views.SettingsViews;

namespace Tathkīr_WPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private static MainWindowViewModel? _instance;
        public static MainWindowViewModel Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new MainWindowViewModel();
                return _instance;
            }
        }

        private object? _selectedTap;
        public object? SelectedTap
        {
            get => _selectedTap;
            set
            {
                if (_selectedTap != value)
                {
                    _selectedTap = value;

                    if (_selectedTap is null)
                        return;

                    var tabItem = (TabItem)_selectedTap;
                    if (tabItem is null)
                        return;

                    if (tabItem.Header?.ToString() == Strings.Home)
                    {
                        CurrentContentControl = new HomeControl();
                    }
                    else if (tabItem.Header?.ToString() == Strings.Athkar)
                    {
                        CurrentContentControl = new AthkarControl();
                    }
                    else if (tabItem.Header?.ToString() == Strings.Quran)
                    {
                        CurrentContentControl = new QuranControl();
                    }
                    else if (tabItem.Header?.ToString() == Strings.Prayer_Times)
                    {
                        CurrentContentControl = new PrayerTimesControl();
                    }
                    else if (tabItem.Header?.ToString() == Strings.Settings)
                    {
                        CurrentContentControl = new SettingsControl();
                    }
                    else
                    {
                        CurrentContentControl = null; 
                    }

                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CurrentContentControl));
                }
            }
        }

        public object? CurrentContentControl { get; set; } = null;

        public bool IsDialogOpen => DialogControl != null;

        private object? _dialogControl;
        public object? DialogControl
        {
            get => _dialogControl;
            set
            {
                _dialogControl = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDialogOpen));
            }
        }

        public MainWindowViewModel()
        {
            TabItem tap = new TabItem();
            tap.Header = Strings.Home;
            SelectedTap = tap;
        }
    }
}
