using Tathkīr_API.DTOs;
using Tathkīr_API.Services.Interfaces;

namespace Tathkīr_API.Services
{
    public class GeoLocationService : IGeoLocationService
    {
        private readonly HttpClient _httpClient;
        private readonly ITranslationService _translator;

        public GeoLocationService(HttpClient httpClient, ITranslationService translator)
        {
            _httpClient = httpClient;
            _translator = translator;
        }

        public async Task<IpResponse> GetDetailsByIpAsync(string ipAddress, string lang = "en")
        {
            var url = $"http://ip-api.com/json/{ipAddress}?fields=status,country,countryCode,regionName,city,timezone";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IpResponse>() ?? new IpResponse { Country = "Unknown", CountryCode = "XX", City = "Unknown" };

            if (lang.StartsWith("en", StringComparison.OrdinalIgnoreCase))
            {
                result.CountryLocalized = result.Country;
                result.CityLocalized = result.City;
            }
            else
            {
                result.CountryLocalized = await _translator.TranslateAsync(result.Country, lang);
                result.CityLocalized = await _translator.TranslateAsync(result.City, lang);
            }

            return result;
        }
    }

}
