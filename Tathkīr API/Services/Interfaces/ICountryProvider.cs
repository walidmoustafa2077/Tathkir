using Tathkīr_API.DTOs;

namespace Tathkīr_API.Services.Interfaces
{
    public interface ICountryProvider
    {
        Task<List<CountryResponse>> GetAllAsync(string lang = "en");
    }

}
