using Microsoft.AspNetCore.Mvc;
using Tathkīr_API.Services;

namespace Tathkīr_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrayerTimesController : ControllerBase
    {
        private readonly IPrayerTimesService _prayerTimesService;

        public PrayerTimesController(IPrayerTimesService prayerTimesService)
        {
            _prayerTimesService = prayerTimesService;
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetToday([FromQuery] string address, [FromQuery] string timeZone)
        {
            if (string.IsNullOrWhiteSpace(address))
                return BadRequest("Address is required.");
            if (string.IsNullOrWhiteSpace(timeZone))
                return BadRequest("Time zone is required.");

            var result = await _prayerTimesService.GetTodayTimingsAsync(address, timeZone);
            return Ok(result);
        }

        [HttpGet("{date}")]
        public async Task<IActionResult> GetByDate(string date, [FromQuery] string address, [FromQuery] string timeZone)
        {
            if (string.IsNullOrWhiteSpace(address))
                return BadRequest("Address is required.");

            var result = await _prayerTimesService.GetTimingsByDateAsync(address, date);
            return Ok(result);
        }
    }
}
