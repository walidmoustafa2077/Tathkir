using System.Collections.ObjectModel;
using Tathkīr_WPF.Services;

namespace Tathkīr_WPF.ViewModels.Settings
{
    public class GeneralSettingsViewModel : ViewModelBase, ISettingsPageService
    {
        public ObservableCollection<string> AvailableLanguages { get; } = new()
        {
            Strings.English, Strings.Arabic
        };

        public ObservableCollection<string> AvailableThemes { get; } = new()
        {
            Strings.Light, Strings.Dark
        };

        private string _selectedLanguage = Strings.Arabic;
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set => SetProperty(ref _selectedLanguage, value);
        }

        private string _selectedTheme = Strings.Light;
        public string SelectedTheme
        {
            get => _selectedTheme;
            set => SetProperty(ref _selectedTheme, value);
        }

        private bool _launchOnStartup = true;
        public bool LaunchOnStartup
        {
            get => _launchOnStartup;
            set => SetProperty(ref _launchOnStartup, value);
        }

        private bool _showNotifications = true;
        public bool ShowNotifications
        {
            get => _showNotifications;
            set => SetProperty(ref _showNotifications, value);
        }

        private bool _showMainWindow = true;
        public bool ShowMainWindow
        {
            get => _showMainWindow;
            set => SetProperty(ref _showMainWindow, value);
        } 

        private bool _showWidget = true;
        public bool ShowWidget
        {
            get => _showWidget;
            set => SetProperty(ref _showWidget, value);
        }

        public GeneralSettingsViewModel()
        {
            // Initialize with default values or load from settings
            SelectedLanguage = SettingsService.AppSettings.AppConfig.Language;
            SelectedTheme = SettingsService.AppSettings.AppConfig.Theme;
            LaunchOnStartup = SettingsService.AppSettings.AppConfig.LaunchOnStartup;
            ShowNotifications = SettingsService.AppSettings.AppConfig.ShowNotifications;
            ShowMainWindow = SettingsService.AppSettings.AppConfig.ShowMainWindow;
            ShowWidget = SettingsService.AppSettings.AppConfig.ShowWidget;
        }

        public void ResetSettings()
        {
            // Reset to default values
            SelectedLanguage = Strings.English;
            SelectedTheme = Strings.Light;

            LaunchOnStartup = true;
            ShowNotifications = true;
            ShowMainWindow = true;
            ShowWidget = true;

            // Save the reset settings
            SaveSettings();
        }

        public void SaveSettings()
        {
            SettingsService.AppSettings.AppConfig.Language = SelectedLanguage;
            SettingsService.AppSettings.AppConfig.Theme = SelectedTheme;

            SettingsService.AppSettings.AppConfig.LaunchOnStartup = LaunchOnStartup;
            SettingsService.AppSettings.AppConfig.ShowNotifications = ShowNotifications;
            SettingsService.AppSettings.AppConfig.ShowMainWindow = ShowMainWindow;
            SettingsService.AppSettings.AppConfig.ShowWidget = ShowWidget;

            // Save the settings to persistent storage
            SettingsService.Save(SettingsService.AppSettings);
        }
    }
}
