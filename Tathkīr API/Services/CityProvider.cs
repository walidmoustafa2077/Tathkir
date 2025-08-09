using Tathkīr_API.DTOs;
using Tathkīr_API.Extensions;
using Tathkīr_API.Services.Interfaces;

namespace Tathkīr_API.Services
{
    public class CityProvider : ICityProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ITranslationService _translator;
        private readonly IKeywordProcessor _keywordProcessor;

        private const string Token = "walidmoustafa";

        private readonly string[] _keywords = new[]
        {
            "Governorate", "Government", "Region", "Province", "State", "Prefecture", "Municipality",
            "District", "Division", "Department", "County", "Emirate", "Territory", "Autonomous Region",
            "Metropolitan", "Administrative", "Federal", "Capital"
        };

        public CityProvider(HttpClient httpClient, ITranslationService translator, IKeywordProcessor keywordProcessor)
        {
            _httpClient = httpClient;
            _translator = translator;
            _keywordProcessor = keywordProcessor;
        }

        public async Task<List<CityResponse>> GetByCountryCodeAsync(string countryCode, string lang = "en")
        {
            var url = $"http://api.geonames.org/searchJSON?country={countryCode}&featureClass=P&maxRows=1000&username={Token}&lang={lang}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var geoResult = await response.Content.ReadFromJsonAsync<GeoCityNamesResponse>();
            if (geoResult?.Geonames == null)
                return new();

            var adminCities = geoResult.Geonames
                .Where(g => g.FcodeName.Contains("administrative", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var keywords = lang.StartsWith("en") ? _keywords : await _translator.TranslateAsync(_keywords, lang);

            var cities = new List<CityResponse>();

            foreach (var g in adminCities)
            {
                var cleanedName = _keywordProcessor.RemoveKeywords(g.Name, keywords);
                var cleanedAdmin = _keywordProcessor.RemoveKeywords(g.AdminName1, keywords);

                bool useAdminFallback = false;
                string translatedAdmin = "";

                if (lang.StartsWith("ar") && g.Name.ContainsLatinLetters())
                    useAdminFallback = true;
                else if (lang.StartsWith("ar"))
                {
                    if (cleanedName.LevenshteinDistance(cleanedAdmin) > 6)
                    {
                        useAdminFallback = true;
                        translatedAdmin = await _translator.TranslateAsync(g.AdminName1, "en");
                    }
                }

                var localized = useAdminFallback ? cleanedAdmin.CapitalizeAlPrefix() : cleanedName.CapitalizeAlPrefix();
                var toponym = useAdminFallback
                    ? _keywordProcessor.RemoveKeywords(translatedAdmin, _keywords).NormalizeName().CapitalizeAlPrefix()
                    : _keywordProcessor.RemoveKeywords(g.ToponymName, _keywords).NormalizeName().CapitalizeAlPrefix();

                cities.Add(new CityResponse
                {
                    NameLocalized = localized,
                    Name = toponym,
                    AdminName1Original = g.AdminName1
                });
            }

            // Associate states
            var stateEntries = geoResult.Geonames
                .Where(g => g.FcodeName.Contains("populated place", StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var city in cities)
            {
                var states = stateEntries
                    .Where(s => s.AdminName1.Equals(city.AdminName1Original, StringComparison.OrdinalIgnoreCase) && !s.Name.ContainsLatinLetters())
                    .Select(s => new StateResponse
                    {
                        Name = _keywordProcessor.RemoveKeywords(s.ToponymName, _keywords).CapitalizeAlPrefix(),
                        NameLocalized = s.Name
                    })
                    .ToList();

                city.States = states;
            }

            return cities.DistinctBy(c => c.Name).ToList();
        }
    }

}
