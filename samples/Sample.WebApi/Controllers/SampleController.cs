using System;
using Microsoft.AspNetCore.Mvc;

namespace Sample.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public class SampleController : ControllerBase
    {
        /// <summary>
        /// Sample 200
        /// </summary>
        [HttpGet("[action]")]
        public IActionResult Sample200() =>
            Ok(new { Healthy = true });

        /// <summary>
        /// Throws 500 Internal Server error
        /// </summary>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(ValidationProblemDetails), 500)]
        public IActionResult Sample500() => throw new Exception("Testing 500");

        /// <summary>
        /// Throws 400 Bad Request on empty body
        /// </summary>
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [HttpPost("[action]")]
        public IActionResult Sample400([FromBody] SampleRequest request)
        {
            return Ok();
        }

        /// <summary>
        /// Throws 404 Not Found driven by a custom exception
        /// </summary>
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [HttpGet("[action]")]
        public IActionResult Custom404() =>
            throw new NotFoundException("Not found");
    }
}