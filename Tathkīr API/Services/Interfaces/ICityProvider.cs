using Tathkīr_API.DTOs;

namespace Tathkīr_API.Services.Interfaces
{
    public interface ICityProvider
    {
        Task<List<CityResponse>> GetByCountryCodeAsync(string countryCode, string lang = "en");
    }

}
