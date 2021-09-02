using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Problem.Details.IntegrationTests.Common;
using Sample.WebApi;
using Shouldly;
using Xunit;

namespace Problem.Details.IntegrationTests
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
            var response = await client.GetAsync("/sample/sample200");

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            await response.BodyShouldBeAsync("{\"healthy\":true}");
        }

        [Fact]
        public async Task WhenNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();
            var expected = "{\"type\":\"https://httpstatuses.com/404\"," +
                           "\"title\":\"Error: Not Found\"," +
                           "\"status\":404}";

            // Act
            var response = await client.GetAsync("/not-found");

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            await response.BodyShouldBeAsync(expected);
        }

        [Fact]
        public async Task WhenBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var expected =
                "{\"type\":\"https://httpstatuses.com/400\"," +
                "\"title\":\"One or more validation errors occurred\"," +
                "\"status\":400," +
                "\"errors\":{\"someField\":[\"The someField field is required.\"]}}";

            // Act
            var response = await client.PostAsync("/sample/sample400",
                new StringContent("{}", Encoding.UTF8, MediaTypeNames.Application.Json));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            await response.BodyShouldBeAsync(expected);
        }

        [Fact]
        public async Task WhenMethodNotAllowed()
        {
            // Arrange
            var client = _factory.CreateClient();
            var expected = "{\"type\":\"https://httpstatuses.com/405\"," +
                           "\"title\":\"Error: Method Not Allowed\"," +
                           "\"status\":405}";

            // Act
            var response = await client.PutAsync("/sample/sample200", new StringContent("{}"));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
            await response.BodyShouldBeAsync(expected);
        }

        [Fact]
        public async Task WhenInternalServerError()
        {
            // Arrange
            var client = _factory.CreateClient();
            var expected = "{\"type\":\"https://httpstatuses.com/500\"," +
                           "\"title\":\"Error: Internal Server Error\"," +
                           "\"status\":500,";

            // Act
            var response = await client.GetAsync("/sample/sample500");
            var body = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            body.TruncateUntil("\"exception\":\"").ShouldBe(expected);
        }

        [Fact]
        public async Task WhenCustomException()
        {
            // Arrange
            var client = _factory.CreateClient();
            var expected = "{\"type\":\"https://httpstatuses.com/404\"," +
                           "\"title\":\"Error: Not Found\"," +
                           "\"status\":404,";

            // Act
            var response = await client.GetAsync("/sample/custom404");
            var body = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            body.TruncateUntil("\"exception\":\"").ShouldBe(expected);
        }
    }
}