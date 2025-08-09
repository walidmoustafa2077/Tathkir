using Tathkīr_API.DTOs;
using Tathkīr_API.Services.Interfaces;

namespace Tathkīr_API.Services
{
    public class PrayerTimesService : IPrayerTimesService
    {
        private readonly HttpClient _httpClient;

        public PrayerTimesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
