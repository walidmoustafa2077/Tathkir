using Tathkīr_API.DTOs;

namespace Tathkīr_API.Services.Interfaces
{
    public interface IGeoLocationService
    {
        Task<IpResponse> GetDetailsByIpAsync(string ipAddress, string lang = "en");
    }

}
