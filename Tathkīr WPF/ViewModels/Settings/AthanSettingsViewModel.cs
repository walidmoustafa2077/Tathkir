using System.Collections.ObjectModel;
using Tathkīr_WPF.Models;

namespace Tathkīr_WPF.ViewModels.Settings
{
    public class AthanSettingsViewModel : ViewModelBase
    {
        public ObservableCollection<PrayerNotificationSetting> BeforeAthanSettings { get; set; }
        public ObservableCollection<PrayerNotificationSetting> OnAthanSettings { get; set; }

        public AthanSettingsViewModel()
        {
            var prayers = new[] { "Fajr", "Dhuhr", "Asr", "Maghrib", "Isha", "Jumua" };

            BeforeAthanSettings = new ObservableCollection<PrayerNotificationSetting>(
                prayers.Select(p => new PrayerNotificationSetting
                {
                    PrayerName = p,
                    IsEnabled = true,
                    SelectedOffset = p == "Jumua" ? 20 : 5,
                    AvailableOffsets = p == "Jumua"
                        ? new List<int> { 20, 30, 45, 60, 100 }
                        : new List<int> { 5, 10, 15, 20, 25, 30 }
                }));

            OnAthanSettings = new ObservableCollection<PrayerNotificationSetting>(
                prayers.Select(p => new PrayerNotificationSetting
                {
                    PrayerName = p,
                    IsEnabled = true,
                }));
        }

    }
}
