using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProblemDetails;

namespace Sample.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(Details), 500)]
    public class WeatherForecastController : ControllerBase
    {
        [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), 200)]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
            => new List<WeatherForecast>
            {
                new()
                {
                    Date = new DateTime(2019, 10, 10),
                    Summary = "First",
                    TemperatureC = 1
                },
                new()
                {
                    Date = new DateTime(2020, 11, 11),
                    Summary = "Second",
                    TemperatureC = 1
                },
                new()
                {
                    Date = new DateTime(2021, 12, 12),
                    Summary = "Third",
                    TemperatureC = 3
                }
            };


        /// <summary>
        /// Throws 500 Internal Server error
        /// </summary>
        [HttpGet("{id:int}")]
        public WeatherForecast Get(int id) => throw new Exception("Testing 500");

        /// <summary>
        /// Throws 400 Bad Request on empty body
        /// </summary>
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationDetails), 400)]
        [HttpPost]
        public IActionResult Post([FromBody] SampleRequest request) => Ok();
    }
}