// =============================
// BlazorSpread.net Sample
// =============================
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MessagePackApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        static readonly Random random = new();

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = random.Next(-20, 55),
                Summary = Summaries[random.Next(Summaries.Length)]
            });
        }

        [HttpGet("{id}")]
        public WeatherForecast Get(int id)
        {
            return new WeatherForecast {
                Date = DateTime.Now,
                TemperatureC = 17 + id,
                Summary = "Cool in London"
            };
        }

        [HttpPost]
        public void Post(WeatherForecast value)
        {
            _logger.LogInformation($"Post argument: {value}");
        }
    }
}
