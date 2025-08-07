using Tathkīr_WPF.Models;

namespace Tathkīr_WPF.ViewModels.Settings.SubViewModels
{
    public class PrayerNotificationSettingViewModel : ViewModelBase
    {
        public string PrayerName { get; set; } = string.Empty;
        public int SelectedOffset { get; set; }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Enabled));
                    OnPropertyChanged(nameof(EnableAlert));
                }
            }
        }

        public string Enabled
        {
            get => _isEnabled ? Strings.Disabled : Strings.Enable;
        }

        public string EnableAlert
        {
            get => _isEnabled ? Strings.Disabled_Alert : Strings.Enable_Alert;
        }

        public List<int> AvailableOffsets { get; set; } = new();

        public PrayerNotificationSettingViewModel() { }
        public PrayerNotificationSettingViewModel(PrayerNotificationSetting model)
        {
            PrayerName = model.PrayerName;
            IsEnabled = model.IsEnabled;
            SelectedOffset = model.SelectedOffset;
        }

        public PrayerNotificationSetting ToModel()
        {
            return new PrayerNotificationSetting
            {
                PrayerName = PrayerName,
                IsEnabled = IsEnabled,
                SelectedOffset = SelectedOffset
            };
        }
    }
}
