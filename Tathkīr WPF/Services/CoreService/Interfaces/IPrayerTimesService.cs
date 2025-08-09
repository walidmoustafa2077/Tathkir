using Tathkīr_WPF.Models;

namespace Tathkīr_WPF.Services.CoreService.Interfaces
{
    public interface IPrayerTimesService
    {
        Task<PrayerTimesResult?> GetPrayerTimesAsync(DateTime date, bool use24HourFormat);
    }
}
