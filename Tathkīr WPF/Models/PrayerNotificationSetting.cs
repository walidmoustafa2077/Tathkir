namespace Tathkīr_WPF.Models
{
    public class PrayerNotificationSetting
    {
        public string PrayerName { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
        public int SelectedOffset { get; set; }
        public List<int> AvailableOffsets { get; set; } = new List<int>();
    }
}
