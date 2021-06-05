using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;

namespace ProblemDetails.IntegrationTests
{
    public static class HttpResponseExtensions
    {
        public static async Task BodyShouldBeAsync(this HttpResponseMessage response, string expected)
        {
            var body = await response.Content.ReadAsStringAsync();
            body.ShouldBe(expected);
        }
    }
}