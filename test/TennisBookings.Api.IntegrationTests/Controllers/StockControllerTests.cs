using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TennisBookings.Api.IntegrationTests.Model;
using TennisBookings.Merchandise.Api;
using Xunit;

namespace TennisBookings.Api.IntegrationTests.Controllers
{
    public class StockControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public StockControllerTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateDefaultClient(new Uri("http://localhost:5120/api/stock"));
        }

        [Fact]
        public async Task GetStockTotal_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("");

            response.EnsureSuccessStatusCode();

        }

        [Fact]
        public async Task GetStockTotal_ReturnsExpectedJsonContentString()
        {
            var response = await _client.GetStringAsync("");

            Assert.Equal("{\"stockItemTotal\":100}", response);
        }

        [Fact]
        public async Task GetStockTotal_ReturnsExpectedJsonContentType()
        {
            var response = await _client.GetAsync("");
            
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task GetStockTotal_ReturnsExpectedJson()
        {
            //all

            var model = await _client.GetFromJsonAsync<ExpectedStockTotalOutputModel>("");

            Assert.NotNull(model);
            Assert.True(model.StockItemTotal > 0);
        }

        [Fact]
        public async Task GetStockTotal_ReturnsExpectedStockQuantity()
        {
            var model = await _client.GetFromJsonAsync<ExpectedStockTotalOutputModel>("");
            
            Assert.Equal(1000, model.StockItemTotal);
        }
    }
}