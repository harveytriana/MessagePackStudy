using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MessagePackApi.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
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

        //[HttpPost]
        //public void Post_T(WeatherForecast value)
        //{
        //    _logger.LogInformation($"{value.Date:dd-MM-yy HH:m:ss} {value.TemperatureC:N2} {value.Summary}");
        //}

        [HttpPost]
        public void Post(string value)
        {
            _logger.LogInformation($"Post argument: {value}");
        }
    }
}
