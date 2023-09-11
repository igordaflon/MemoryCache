using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MemoryCache.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMemoryCache _memoryCache;
        private const string KEY = "KeyTeste";

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
                                         IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }


        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            if(_memoryCache.TryGetValue(KEY, out IEnumerable<string>? summaries))
            {
                return Ok(summaries);
            }

            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60),
                SlidingExpiration = TimeSpan.FromMinutes(20)
            };

            summaries = Summaries;
            _memoryCache.Set(KEY, summaries, memoryCacheEntryOptions);

            return Ok(summaries);
        }
    }
}