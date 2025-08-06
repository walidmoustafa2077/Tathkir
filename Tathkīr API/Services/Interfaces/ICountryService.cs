using Tathkīr_API.DTOs;

namespace Tathkīr_API.Services.Interfaces
{
    public interface ICountryService
    {
        Task<IpResponse> GetCountryDetailsByIpAsync(string ipAddress, string lang = "en");
        Task<List<CountryResponse>> GetAllCountriesAsync(string lang = "en");
        Task<List<CityResponse>> GetCountryDetailsFromIpAsync(string ipAddress, string lang = "en");
        Task<List<CityResponse>> GetCountryDetailsAsync(string countryCode, string lang = "en");
    }

}
