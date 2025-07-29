using Tathkīr_API.Services;

namespace Tathkīr_API.DTOs
{
    public class PrayerTimesService : IPrayerTimesService
    {
        private readonly HttpClient _httpClient;

        public PrayerTimesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PrayerTimingsResponse> GetTodayTimingsAsync(string address, string timeZone)
        {
            // Get time zone info
            var tzInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            var todayLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzInfo);

            // Pass correct date in local time zone
            var date = todayLocal.ToString("dd-MM-yyyy");

            return await GetTimingsByDateAsync(address, date);
        }

        public async Task<PrayerTimingsResponse> GetTimingsByDateAsync(string address, string date)
        {
            var url = $"https://api.aladhan.com/v1/timingsByAddress/{date}" +
                      $"?address={Uri.EscapeDataString(address)}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PrayerTimingsResponse>();
            if (result == null)
                throw new Exception("Failed to fetch prayer timings.");

            return result;
        }
    }

}
