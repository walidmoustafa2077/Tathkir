using Tathkīr_API.DTOs;

namespace Tathkīr_API.Services
{
    public interface IPrayerTimesService
    {
        Task<PrayerTimingsResponse> GetTodayTimingsAsync(string address, string timeZone);
        Task<PrayerTimingsResponse> GetTimingsByDateAsync(string address, string date);
    }

}
