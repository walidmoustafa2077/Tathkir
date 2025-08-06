using System.Net.Http;
using System.Text.Json;
using Tathkīr_WPF.Models;

namespace Tathkīr_WPF.Services
{
    public class HostService
    {
        private static HostService _instance = null!;
        public static HostService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HostService();
                }
                return _instance;
            }
        }

        private readonly HttpClient _httpClient;
        
        private const string BaseUrl = "https://localhost:7299/api";

        private List<Country> _countries = new List<Country>();
        private List<City> _cities = new List<City>();

        public HostService(HttpClient? httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
        }

        public async Task<Address> GetAddress(string lang = "en")
        {
            try
            {
                string url = $"{BaseUrl}/Main/details-by-ip?lang={lang}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Address>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new Address();
            }
            catch (Exception e)
            {
                // Log or handle JSON deserialization errors
                DialogService.Instance.ShowError(Strings.Error, "Failed to load address data. Please try again later." + e.Message);
                return new Address();
            }
        }

        public async Task<PrayerTimesResponse?> GetTodayPrayerTimesAsync(DateTime date)
        {
            try
            {
                string formattedDate = date.ToString("dd-MM-yyyy");
                string url = $"{BaseUrl}/PrayerTimes/{formattedDate}?address={Uri.EscapeDataString(SettingsService.AppSettings.ApiConfig.Address)}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<PrayerTimesResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception e)
            {
                // Log or handle JSON deserialization errors
                DialogService.Instance.ShowError(Strings.Error, "Failed to load prayer times data. Please try again later." + e.Message);
                return null;
            }
        }

        public async Task<List<string>> GetCountryNamesAsync()
        {
            _countries = await GetCountriesAsync();
            return _countries.Select(c => c.NameLocalized).ToList();
        }

        public string GetCurrentAddress(string? selectedCountry, string? selectedCity)
        {
            var country = _countries.FirstOrDefault(c => c.NameLocalized.Equals(selectedCountry, StringComparison.OrdinalIgnoreCase));
            var city = _cities.FirstOrDefault(c => c.NameLocalized.Equals(selectedCity, StringComparison.OrdinalIgnoreCase));

            if (country == null || city == null)
            {
                return string.Empty;
            }

            return $"{city.Name}, {country.Code})";
        }

        private async Task<List<Country>> GetCountriesAsync()
        {
            try
            {
                var lang = App.IsRtl ? "ar" : "en";

                string url = $"{BaseUrl}/Main/countries?lang={lang}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<List<Country>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Country>();
            }
            catch (Exception e)
            {
                // Log or handle JSON deserialization errors
                DialogService.Instance.ShowError(Strings.Error, "Failed to load countries data. Please try again later." + e.Message);
                return new List<Country>();
            }
        }

        public async Task<List<string>> GetCitiesByCountryNameAsync(string countryName)
        {
            _cities = await GetCitiesByCountryAsync(countryName);
            return _cities.Select(c => c.NameLocalized).ToList();
        }

        private async Task<List<City>> GetCitiesByCountryAsync(string countryName)
        {
            try
            {
                var code = _countries.FirstOrDefault(c => c.NameLocalized.Equals(countryName, StringComparison.OrdinalIgnoreCase))?.Code;
                var lang = App.IsRtl ? "ar" : "en";

                if (string.IsNullOrEmpty(code))
                {
                    return new List<City>();
                }

                string url = $"{BaseUrl}/Main/country-details?code={code}&lang={lang}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<List<City>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<City>();
            }
            catch (Exception e)
            {
                // Log or handle JSON deserialization errors
                DialogService.Instance.ShowError(Strings.Error, "Failed to load cities data. Please try again later." + e.Message);
                return new List<City>();
            }
        }
    }
}
