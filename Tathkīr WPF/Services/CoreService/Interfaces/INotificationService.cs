using Tathkīr_WPF.Models;

namespace Tathkīr_WPF.Services.CoreService.Interfaces
{
    public interface INotificationService
    {
        void NotifyBeforePrayer(PrayerItem prayer, string? audioPath);
        void NotifyOnPrayer(PrayerItem prayer, string? audioPath);
        void NotifyThikr(AthkarItem thikr, string? audioPath);
    }
}
