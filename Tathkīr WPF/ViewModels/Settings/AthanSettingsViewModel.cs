using System.Collections.ObjectModel;
using Tathkīr_WPF.Extensions;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services;

namespace Tathkīr_WPF.ViewModels.Settings
{
    public class AthanSettingsViewModel : ViewModelBase
    {
        public ObservableCollection<PrayerNotificationSetting> BeforeAthanSettings { get; set; } = new ObservableCollection<PrayerNotificationSetting>();
        public ObservableCollection<PrayerNotificationSetting> OnAthanSettings { get; set; } = new ObservableCollection<PrayerNotificationSetting>();

        public AthanSettingsViewModel()
        {
            var prayers = new[] { Strings.Fajr, Strings.Dhuhr, Strings.Asr, Strings.Maghrib, Strings.Isha, Strings.Jumua };

            if (SettingsService.AppSettings.AppConfig.AthanConfigSettings == null)
            {
                SettingsService.AppSettings.AppConfig.AthanConfigSettings = SettingsService.AppSettings.AppConfig.SetAthanConfigSettings();

                BeforeAthanSettings = new ObservableCollection<PrayerNotificationSetting>(
                    prayers.Select(p => new PrayerNotificationSetting
                    {
                        PrayerName = p,
                        IsEnabled = true,
                        SelectedOffset = p == Strings.Jumua ? 20 : 5,
                        AvailableOffsets = p == Strings.Jumua
                            ? new List<int> { 20, 30, 45, 60, 100 }
                            : new List<int> { 5, 10, 15, 20, 25, 30 }
                    }));

                // Save the default settings to the app config
                SettingsService.AppSettings.AppConfig.AthanConfigSettings.BeforeAthanSettings = BeforeAthanSettings;

                foreach (var item in SettingsService.AppSettings.AppConfig.AthanConfigSettings.BeforeAthanSettings)
                    item.PrayerName = item.PrayerName.ToInvariantLanguage();

                OnAthanSettings = new ObservableCollection<PrayerNotificationSetting>(
                prayers.Select(p => new PrayerNotificationSetting
                {
                    PrayerName = p,
                    IsEnabled = true,
                }));

                // Save the default settings to the app config
                SettingsService.AppSettings.AppConfig.AthanConfigSettings.OnAthanSettings = OnAthanSettings;

                foreach (var item in SettingsService.AppSettings.AppConfig.AthanConfigSettings.OnAthanSettings)
                    item.PrayerName = item.PrayerName.ToInvariantLanguage();

                // Save the settings to the service
                SettingsService.Save(SettingsService.AppSettings);
            }
            else
            {
                BeforeAthanSettings = SettingsService.AppSettings.AppConfig.AthanConfigSettings.BeforeAthanSettings;
            
                foreach (var item in BeforeAthanSettings)
                    item.PrayerName = item.PrayerName.ToLocalizedLanguage();
                
                OnAthanSettings = SettingsService.AppSettings.AppConfig.AthanConfigSettings.OnAthanSettings;
                
                foreach (var item in OnAthanSettings)
                    item.PrayerName = item.PrayerName.ToLocalizedLanguage();
            }
        }

        public void SaveSettings()
        {
            if (SettingsService.AppSettings.AppConfig.AthanConfigSettings == null)
            {
                SettingsService.AppSettings.AppConfig.AthanConfigSettings = SettingsService.AppSettings.AppConfig.SetAthanConfigSettings();
            }

            // Save the default settings to the app config
            SettingsService.AppSettings.AppConfig.AthanConfigSettings.BeforeAthanSettings = BeforeAthanSettings;

            foreach (var item in SettingsService.AppSettings.AppConfig.AthanConfigSettings.BeforeAthanSettings)
                item.PrayerName = item.PrayerName.ToInvariantLanguage();

            // Save the default settings to the app config
            SettingsService.AppSettings.AppConfig.AthanConfigSettings.OnAthanSettings = OnAthanSettings;

            foreach (var item in SettingsService.AppSettings.AppConfig.AthanConfigSettings.OnAthanSettings)
                item.PrayerName = item.PrayerName.ToInvariantLanguage();

            // Save the settings to the service
            SettingsService.Save(SettingsService.AppSettings);
        }
    }
}
