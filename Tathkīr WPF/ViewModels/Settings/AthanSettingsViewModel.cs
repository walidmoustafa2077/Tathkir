using System.Collections.ObjectModel;
using Tathkīr_WPF.Services;
using Tathkīr_WPF.ViewModels.Settings.SubViewModels;

namespace Tathkīr_WPF.ViewModels.Settings
{
    public class AthanSettingsViewModel : ViewModelBase, ISettingsPageService
    {
        private ObservableCollection<PrayerNotificationSettingViewModel> _beforeAthanSettings = new ObservableCollection<PrayerNotificationSettingViewModel>();
        public ObservableCollection<PrayerNotificationSettingViewModel> BeforeAthanSettings
        {
            get => _beforeAthanSettings;
            set
            {
                if (_beforeAthanSettings != value)
                {
                    _beforeAthanSettings = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<PrayerNotificationSettingViewModel> _onAthanSettings = new ObservableCollection<PrayerNotificationSettingViewModel>();
        public ObservableCollection<PrayerNotificationSettingViewModel> OnAthanSettings
        {
            get => _onAthanSettings;
            set
            {
                if (_onAthanSettings != value)
                {
                    _onAthanSettings = value;
                    OnPropertyChanged();
                }
            }
        }

        public AthanSettingsViewModel()
        {

            if (SettingsService.AppSettings.AppConfig.AthanConfigSettings == null)
                Initialize();
            else
            {

                BeforeAthanSettings = new ObservableCollection<PrayerNotificationSettingViewModel>(
                    SettingsService.AppSettings.AppConfig.AthanConfigSettings.BeforeAthanSettings
                        .Select(model => new PrayerNotificationSettingViewModel(model)));

                OnAthanSettings = new ObservableCollection<PrayerNotificationSettingViewModel>(
                    SettingsService.AppSettings.AppConfig.AthanConfigSettings.OnAthanSettings
                        .Select(model => new PrayerNotificationSettingViewModel(model)));

                foreach (var item in BeforeAthanSettings)
                {
                    item.AvailableOffsets = item.PrayerName == Strings.Jumua
                            ? new List<int> { 20, 30, 45, 60, 100 }
                            : new List<int> { 5, 10, 15, 20, 25, 30 };
                }


            }
        }

        private void Initialize()
        {
            BeforeAthanSettings.Clear();
            OnAthanSettings.Clear();

            var prayers = new[] { Strings.Fajr, Strings.Dhuhr, Strings.Asr, Strings.Maghrib, Strings.Isha, Strings.Jumua };

            BeforeAthanSettings = new ObservableCollection<PrayerNotificationSettingViewModel>(
                prayers.Select(p => new PrayerNotificationSettingViewModel
                {
                    PrayerName = p,
                    IsEnabled = true,
                    SelectedOffset = p == Strings.Jumua ? 20 : 5
                }));

            OnAthanSettings = new ObservableCollection<PrayerNotificationSettingViewModel>(
            prayers.Select(p => new PrayerNotificationSettingViewModel
            {
                PrayerName = p,
                IsEnabled = true,
            }));


            foreach (var item in BeforeAthanSettings)
            {
                item.AvailableOffsets = item.PrayerName == Strings.Jumua
                        ? new List<int> { 20, 30, 45, 60, 100 }
                        : new List<int> { 5, 10, 15, 20, 25, 30 };
            }

            // Save the settings to the service
            SaveSettings();
        }

        public void ResetSettings() => Initialize();

        public void SaveSettings()
        {
            if (SettingsService.AppSettings.AppConfig.AthanConfigSettings == null)
            {
                SettingsService.AppSettings.AppConfig.AthanConfigSettings = SettingsService.AppSettings.AppConfig.SetAthanConfigSettings();
            }

            // Convert ViewModels to Models before saving
            SettingsService.AppSettings.AppConfig.AthanConfigSettings.BeforeAthanSettings =
                BeforeAthanSettings.Select(vm => vm.ToModel()).ToList();

            SettingsService.AppSettings.AppConfig.AthanConfigSettings.OnAthanSettings =
                OnAthanSettings.Select(vm => vm.ToModel()).ToList();

            // Save the settings to the service
            SettingsService.Save(SettingsService.AppSettings);
        }
    }
}
