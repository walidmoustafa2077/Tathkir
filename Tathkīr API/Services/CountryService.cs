using System.Text.Encodings.Web;
using System.Text.Json;
using Tathkīr_API.DTOs;
using Tathkīr_API.Services.Interfaces;

namespace Tathkīr_API.Services
{
    public class CountryService : ICountryService
    {
        private readonly IGeoLocationService _geoLocationService;
        private readonly ICountryProvider _countryProvider;
        private readonly ICityProvider _cityProvider;

        public CountryService(IGeoLocationService geoLocationService, ICountryProvider countryProvider, ICityProvider cityProvider)
        {
            _geoLocationService = geoLocationService;
            _countryProvider = countryProvider;
            _cityProvider = cityProvider;
        }

        public Task<IpResponse> GetCountryDetailsByIpAsync(string ipAddress, string lang = "en")
        {
            return _geoLocationService.GetDetailsByIpAsync(ipAddress, lang);
        }

        public async Task<List<CountryResponse>> GetAllCountriesAsync(string lang = "en")
        {
            return await GetCountriesCachedAsync(lang);
        }

        public async Task<List<CityResponse>> GetCountryDetailsAsync(string code, string lang = "en")
        {
            return await GetCitiesCachedAsync(code, lang);
        }

        public async Task<List<CityResponse>> GetCountryDetailsFromIpAsync(string ip, string lang = "en")
        {
            var result = await GetCountryDetailsByIpAsync(ip, lang);
            return await GetCitiesCachedAsync(result.CountryCode, lang);
        }

        public async Task<List<CountryResponse>> GetCountriesCachedAsync(string lang = "en")
        {
            var filePath = GetCacheFilePath("countries", "all", lang);
            List<CountryResponse>? countries = null;

            if (File.Exists(filePath))
            {
                var json = await File.ReadAllTextAsync(filePath);

                countries = JsonSerializer.Deserialize<List<CountryResponse>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (countries != null && countries.Count > 0)
                {
                    return countries;
                }
            }

            countries = await _countryProvider.GetAllAsync(lang);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(countries, options));
            return countries;
        }

        public async Task<List<CityResponse>> GetCitiesCachedAsync(string countryCode, string lang = "en")
        {
            var filePath = GetCacheFilePath("cities", countryCode, lang);
            List<CityResponse>? cities = null;
            if (File.Exists(filePath))
            {
                var json = await File.ReadAllTextAsync(filePath);
                cities = JsonSerializer.Deserialize<List<CityResponse>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (cities != null && cities.Count > 0)
                {
                    return cities;
                }
            }

            // Cache miss — generate
            cities = await _cityProvider.GetByCountryCodeAsync(countryCode, lang);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(cities, options));
            return cities;
        }

        private static string GetCacheFilePath(string type, string key, string lang)
        {
            string Sanitize(string input)
            {
                foreach (var c in Path.GetInvalidFileNameChars())
                    input = input.Replace(c, '_');
                return input;
            }

            var safeKey = Sanitize(key);
            var safeLang = Sanitize(lang);
            var fileName = $"{type}_{safeKey}_{safeLang}.json";

            return Path.Combine(AppContext.BaseDirectory, "Cache", fileName);
        }

    }
}
