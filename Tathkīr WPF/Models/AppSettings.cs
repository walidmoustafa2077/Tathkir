using System.Collections.ObjectModel;

namespace Tathkīr_WPF.Models
{
    public class AppSettings
    {
        public string LastSelectedReader { get; set; } = string.Empty;

        public ApiConfigSettings ApiConfig { get; set; } = new ApiConfigSettings();

        public AppConfigSettings AppConfig { get; set; } = new AppConfigSettings();
    }

    public class ApiConfigSettings
    {
        public string Country { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; } = DateTime.MinValue;
    }

    public class AppConfigSettings
    {
        public string Language { get; set; } = string.Empty;
        public string Theme { get; set; } = string.Empty;
        public bool LaunchOnStartup { get; set; } = true;
        public bool ShowNotifications { get; set; } = true;

        public AthanSettings? AthanConfigSettings { get; set; } = null;
        public AthkarSettings? AthkarConfigSettings { get; set; } = null;

        public AthanSettings SetAthanConfigSettings()
        {
            return new AthanSettings();
        }

        public AthkarSettings SetAthkarConfigSettings()
        {
            return new AthkarSettings();
        }

        public class AthanSettings
        {
            public ObservableCollection<PrayerNotificationSetting> BeforeAthanSettings { get; set; } = new ObservableCollection<PrayerNotificationSetting>();
            public ObservableCollection<PrayerNotificationSetting> OnAthanSettings { get; set; } = new ObservableCollection<PrayerNotificationSetting>();
        }

        public class AthkarSettings
        {
            public ObservableCollection<AthkarItem> AthkarList { get; set; } = new ObservableCollection<AthkarItem>();
        }
    }
}
