using Tathkīr_WPF.Extensions;
using Tathkīr_WPF.Managers;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services.CoreService.Interfaces;

namespace Tathkīr_WPF.Services.CoreService
{
    public class NotificationService : INotificationService
    {
        public void NotifyBeforePrayer(PrayerItem prayer, string? audioPath)
        {
            ToastMaganer.ShowToast(
                Strings.Prayer_Reminder,
                $"{Strings.The_Time_For} {prayer.Name} {Strings.Prayer_Is_Approaching}",
                audioPath);
        }

        public void NotifyOnPrayer(PrayerItem prayer, string? audioPath)
        {
            ToastMaganer.ShowToast(
                Strings.Prayer_Reminder,
                $"{Strings.It_s_Time_For} {prayer.Name} {Strings.Prayer}",
                audioPath);
        }

        public void NotifyThikr(AthkarItem thikr, string? audioPath)
        {
            ToastMaganer.ShowToast(
                Strings.Thikr_Reminder,
                thikr.Name.ToLocalizedLanguage(),
                audioPath);
        }
    }

}
