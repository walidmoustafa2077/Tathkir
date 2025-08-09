using System.Collections.ObjectModel;
using Tathkīr_WPF.Extensions;

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
        public bool ShowMainWindow { get; set; } = true;
        public bool ShowWidget { get; set; } = true;

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

        public void OnSave()
        {
            foreach (var item in AthanConfigSettings?.BeforeAthanSettings ?? new List<PrayerNotificationSetting>())
            {
                item.PrayerName = item.PrayerName.ToInvariantLanguage();
            }

            foreach (var item in AthanConfigSettings?.OnAthanSettings ?? new List<PrayerNotificationSetting>())
            {
                item.PrayerName = item.PrayerName.ToInvariantLanguage();
            }

            foreach (var item in AthkarConfigSettings?.AthkarList ?? new ObservableCollection<AthkarItem>())
            {
                item.Name = item.Name.ToInvariantLanguage();
            }

            Language = Language.ToInvariantLanguage();
            Theme = Theme.ToInvariantLanguage();
        }

        public void Onload()
        {
            foreach (var item in AthanConfigSettings?.BeforeAthanSettings ?? new List<PrayerNotificationSetting>())
            {
                item.PrayerName = item.PrayerName.ToLocalizedLanguage();
            }

            foreach (var item in AthanConfigSettings?.OnAthanSettings ?? new List<PrayerNotificationSetting>())
            {
                item.PrayerName = item.PrayerName.ToLocalizedLanguage();
            }

            foreach (var item in AthkarConfigSettings?.AthkarList ?? new ObservableCollection<AthkarItem>())
            {
                item.Name = item.Name.ToLocalizedLanguage();
            }

            Language = Language.ToLocalizedLanguage();
            Theme = Theme.ToLocalizedLanguage();
        }

        public class AthanSettings
        {
            public List<PrayerNotificationSetting> BeforeAthanSettings { get; set; } = new List<PrayerNotificationSetting>();
            public List<PrayerNotificationSetting> OnAthanSettings { get; set; } = new List<PrayerNotificationSetting>();
        }

        public class AthkarSettings
        {
            public ObservableCollection<AthkarItem> AthkarList { get; set; } = new ObservableCollection<AthkarItem>();
        }
    }
}
