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
                        CurrentContentControl = _homeControl;
                    }
                    else if (tabItem.Header?.ToString() == Strings.Athkar)
                    {
                        CurrentContentControl = _athkarControl;
                    }
                    else if (tabItem.Header?.ToString() == Strings.Quran)
                    {
                        CurrentContentControl = _quranControl;
                    }
                    else if (tabItem.Header?.ToString() == Strings.Prayer_Times)
                    {
                        CurrentContentControl = _prayerTimesControl;
                    }
                    else if (tabItem.Header?.ToString() == Strings.Settings)
                    {
                        CurrentContentControl = _settingsControl;
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

        private HomeControl? _homeControl;
        private AthkarControl? _athkarControl;
        private QuranControl? _quranControl;
        private PrayerTimesControl? _prayerTimesControl;
        private SettingsControl? _settingsControl;


        public MainWindowViewModel()
        {
            _homeControl ??= new HomeControl();
            _athkarControl ??= new AthkarControl();
            _quranControl ??= new QuranControl();
            _prayerTimesControl ??= new PrayerTimesControl();
            _settingsControl ??= new SettingsControl();


            TabItem tap = new TabItem();
            tap.Header = Strings.Home;
            SelectedTap = tap;
        }
    }
}
