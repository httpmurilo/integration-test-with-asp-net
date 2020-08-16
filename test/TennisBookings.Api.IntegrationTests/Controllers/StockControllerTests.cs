using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TennisBookings.Api.IntegrationTests.Fakes;
using TennisBookings.Api.IntegrationTests.Model;
using TennisBookings.Merchandise.Api;
using TennisBookings.Merchandise.Api.Data.Dto;
using TennisBookings.Merchandise.Api.External.Database;
using Xunit;

namespace TennisBookings.Api.IntegrationTests.Controllers
{
    public class StockControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly  WebApplicationFactory<Startup> _factory;

        public StockControllerTests(WebApplicationFactory<Startup> factory)
        {
            
            factory.ClientOptions.BaseAddress = (new Uri("http://localhost:5120/api/stock"));
            _client = factory.CreateDefaultClient();
            _factory = factory;
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

            var cloudDatabase = new FakeCloudDatabase(new[]
            {
                new ProductDto{ StockCount = 200},
                new ProductDto{ StockCount = 500},
                new ProductDto{ StockCount = 300}
            });

            var client = _factory.WithWebHostBuilder(builder => 
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<ICloudDatabase>(cloudDatabase);
                });
            }).CreateClient();
            var model = await _client.GetFromJsonAsync<ExpectedStockTotalOutputModel>("");

            Assert.Equal(1000, model.StockItemTotal);
        }
    }
}