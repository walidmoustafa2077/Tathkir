using Microsoft.AspNetCore.Mvc;
using Tathkīr_API.Services;

namespace Tathkīr_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public MainController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetCountryDetails([FromQuery] string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                return BadRequest("IP address is required.");

            var details = await _countryService.GetCountryDetailsByIpAsync(ip);
            return Ok(details);
        }
    }
}
