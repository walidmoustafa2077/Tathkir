using Tathkīr_API.DTOs;

namespace Tathkīr_API.Services
{
    public class CountryService : ICountryService
    {
        private readonly HttpClient _httpClient;

        public CountryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IpApiResponse> GetCountryDetailsByIpAsync(string ipAddress)
        {
            // Example: https://ip-api.com/json/94.200.17.192
            var url = $"http://ip-api.com/json/{ipAddress}?fields=status,country,countryCode,regionName,city,timezone";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IpApiResponse>();

            if (result?.Status != "success")
                throw new Exception("Failed to retrieve data from ip-api.com");

            return result;
        }


    }
}
