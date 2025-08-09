using Tathkīr_API.DTOs;

namespace Tathkīr_API.Services.Interfaces
{
    public interface IPrayerTimesService
    {
        Task<PrayerTimingsResponse> GetTimingsByDateAsync(string address, string date);
    }

}
