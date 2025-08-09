using Tathkīr_API.DTOs;
using Tathkīr_API.Services.Interfaces;

namespace Tathkīr_API.Services
{
    public class CountryProvider : ICountryProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ITranslationService _translator;
        private const string Token = "walidmoustafa";

        public CountryProvider(HttpClient httpClient, ITranslationService translator)
        {
            _httpClient = httpClient;
            _translator = translator;
        }

        public async Task<List<CountryResponse>> GetAllAsync(string lang = "en")
        {
            var url = $"http://api.geonames.org/countryInfoJSON?username={Token}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var geoResult = await response.Content.ReadFromJsonAsync<GeoCountryInfoResponse>();
            if (geoResult?.Geonames == null)
                return new();

            var countriesToIgnore = new HashSet<string> { "IL" };

            var filtered = geoResult.Geonames
                .Where(c => !string.IsNullOrWhiteSpace(c.CountryCode) && !countriesToIgnore.Contains(c.CountryCode))
                .ToList();

            var namesToTranslate = lang.StartsWith("en") ? null : await _translator.TranslateAsync(filtered.Select(c => c.CountryName), lang);

            var result = filtered.Select((c, i) => new CountryResponse
            {
                Name = c.CountryName,
                NameLocalized = lang.StartsWith("en") ? c.CountryName : namesToTranslate?[i] ?? c.CountryName,
                Code = c.CountryCode.ToUpperInvariant()
            });

            return result.OrderBy(x => x.Name).ToList();
        }
    }

}
