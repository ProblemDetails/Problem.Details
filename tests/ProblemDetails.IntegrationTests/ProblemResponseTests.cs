using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Sample.WebApi;
using Shouldly;
using Xunit;
using Xunit.Sdk;

namespace ProblemDetails.IntegrationTests
{
    public class ProblemResponseTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ProblemResponseTests(WebApplicationFactory<Startup> factory)
            => _factory = factory;

        [Fact]
        public async Task WhenSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/weatherforecast");

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            await response.BodyShouldBeAsync(
                "[{\"Date\":\"2019-10-10T00:00:00\",\"TemperatureC\":1,\"TemperatureF\":33,\"Summary\":\"First\"},{\"Date\":\"2020-11-11T00:00:00\",\"TemperatureC\":1,\"TemperatureF\":33,\"Summary\":\"Second\"},{\"Date\":\"2021-12-12T00:00:00\",\"TemperatureC\":3,\"TemperatureF\":37,\"Summary\":\"Third\"}]");
        }
        
        [Fact]
        public async Task WhenNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/not-found");

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            await response.BodyShouldBeAsync(
                "{\"type\":\"https://httpstatuses.com/404\",\"title\":\"Error 404\",\"status\":404}");
        }

        [Fact]
        public async Task WhenBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/weatherforecast",
                new StringContent("{}", Encoding.UTF8, MediaTypeNames.Application.Json));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            await response.BodyShouldBeAsync(
                "{\"errors\":{\"requiredField\":[\"The requiredField field is required.\"]},\"type\":\"https://httpstatuses.com/400\",\"title\":\"One or more validation errors occurred.\",\"status\":400}"
            );
        }

        [Fact]
        public async Task WhenMethodNotAllowed()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PutAsync("/weatherforecast/123", new StringContent("{}"));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
            await response.BodyShouldBeAsync(
                "{\"type\":\"https://httpstatuses.com/405\",\"title\":\"Error 405\",\"status\":405}");
        }

        [Fact]
        public async Task WhenInternalServerError()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/weatherforecast/123");

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            await response.BodyShouldBeAsync(
                "{\"type\":\"https://httpstatuses.com/500\",\"title\":\"500, Oops!\",\"status\":500}");
        }
    }
}