using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TennisBookings.Merchandise.Api;
using Xunit;

namespace TennisBookings.Api.IntegrationTests.Controllers
{
    public class CategoriesControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        
        public CategoriesControllerTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task GetAll_ReturnsSucessStatusCode()
        {
            var response = await _client.GetAsync("/api/categories");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAll_ReturnsExpectedMediaType()
        {
            var response = await _client.GetAsync("/api/categories");

            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task GetlAll_returnsContent()
        {
           var response = await _client.GetAsync("/api/categories");

            Assert.NotNull(response.Content);
            Assert.True(response.Content.Headers.ContentLength > 0);
        }

        [Fact]
        public async Task GetAll_ReturnsExpectedJson()
            {
                var response = await _client.GetStringAsync("/api/categories");

                Assert.Equal("{\"allowedCategories\":[\"Accessories\"," +
                    "\"Bags\",\"Balls\",\"Clothing\",\"Rackets\"]}", response);
            }
        
    }
}