using Microsoft.AspNetCore.Mvc;
using Tathkīr_API.Services.Interfaces;

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

        [HttpGet("details-by-ip")]
        public async Task<IActionResult> GetCountryDetailsByIp([FromQuery] string lang = "en")
        {
            string? ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            }

            if (ip == "::1" || ip == "127.0.0.1")
            {
                // Substitute with a known external IP for testing purposes
                ip = "41.47.110.213";
            }

            if (string.IsNullOrWhiteSpace(ip))
                return BadRequest("IP address is required.");

            var details = await _countryService.GetCountryDetailsByIpAsync(ip, lang);
            return Ok(details);
        }

        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries([FromQuery] string lang = "en")
        {
            var countries = await _countryService.GetAllCountriesAsync(lang);
            return Ok(countries);
        }

        [HttpGet("country-details")]
        public async Task<IActionResult> GetCountryDetails([FromQuery] string code, [FromQuery] string lang = "en")
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("Country code is required.");
            var details = await _countryService.GetCountryDetailsAsync(code.ToUpperInvariant(), lang: lang);
            return Ok(details);
        }

        [HttpGet("country-details-by-ip")]
        public async Task<IActionResult> GetCountryDetails([FromQuery] string lang = "en")
        {
            string? ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            }

            if (ip == "::1" || ip == "127.0.0.1")
            {
                // Substitute with a known external IP for testing purposes
                ip = "41.47.110.213";
            }

            if (string.IsNullOrWhiteSpace(ip))
                return BadRequest("IP address is required.");

            var details = await _countryService.GetCountryDetailsFromIpAsync(ip, lang);
            return Ok(details);
        }

    }
}
