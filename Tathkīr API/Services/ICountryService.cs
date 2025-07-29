using Tathkīr_API.DTOs;

namespace Tathkīr_API.Services
{
    public interface ICountryService
    {
        Task<IpApiResponse> GetCountryDetailsByIpAsync(string ipAddress);
    }

}
