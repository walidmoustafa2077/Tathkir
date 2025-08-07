using Tathkīr_WPF.Models;

namespace Tathkīr_WPF.Services.CoreService.Interfaces
{
    public interface IPrayerScheduler
    {
        PrayerItem? GetNextPrayer(IEnumerable<PrayerItem> prayers, DateTime now, out DateTime? nextPrayerTime);
    }
}
