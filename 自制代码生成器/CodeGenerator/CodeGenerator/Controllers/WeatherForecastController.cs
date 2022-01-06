using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeGenerator.CodeGenerator.DbFirst;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zxw.Framework.NetCore.IDbContext;

namespace CodeGenerator.Controllers
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
        IDbFirst _dbFirst;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDbFirst dbFirst)
        {
            _logger = logger;
            _dbFirst = dbFirst;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _dbFirst.GenerateAll(true);
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
