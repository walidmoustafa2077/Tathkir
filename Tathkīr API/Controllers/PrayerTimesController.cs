using Microsoft.AspNetCore.Mvc;
using Tathkīr_API.Services.Interfaces;

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


        [HttpGet("{date}")]
        public async Task<IActionResult> GetByDate(string date, [FromQuery] string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return BadRequest("Address is required.");

            var result = await _prayerTimesService.GetTimingsByDateAsync(address, date);
            return Ok(result);
        }
    }
}
