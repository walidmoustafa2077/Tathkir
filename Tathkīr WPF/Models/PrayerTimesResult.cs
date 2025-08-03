namespace Tathkīr_WPF.Models
{
    public class PrayerTimesResult
    {
        public List<PrayerItem> Prayers { get; set; } = new();
        public string Midnight { get; set; } = string.Empty;
        public string LastThird { get; set; } = string.Empty;
        public string CurrentDate { get; set; } = string.Empty;
    }

}
