using System.Net.Http;
using System.Text.Json;
using Tathkīr_WPF.Models;

namespace Tathkīr_WPF.Services
{
    public class PrayerTimesService
    {
        private readonly HttpClient _httpClient;

        public PrayerTimesService(HttpClient? httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
        }

        public async Task<PrayerTimesResponse?> GetTodayPrayerTimesAsync(DateTime date, string address)
        {
            string formattedDate = date.ToString("dd-MM-yyyy");
            string url = $"http://localhost:5000/api/PrayerTimes/{formattedDate}?address={Uri.EscapeDataString(address)}&timeZone={Uri.EscapeDataString("timeZone")}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PrayerTimesResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
