using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TennisBookings.Api.IntegrationTests.Model;
using TennisBookings.Api.IntegrationTests.TestHelpers;
using TennisBookings.Merchandise.Api;
using Xunit;

namespace TennisBookings.Api.IntegrationTests.Controllers
{
    public class CategoriesControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        
        public CategoriesControllerTests(WebApplicationFactory<Startup> factory)
        {
           // _client = factory.CreateDefaultClient(new Uri("http://localhost:5120/api/categories"));
            factory.ClientOptions.BaseAddress = new Uri("http://localhost:5120/api/categories");
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ReturnsSucessStatusCode()
        {
            var response = await _client.GetAsync("");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAll_ReturnsExpectedMediaType()
        {
            var response = await _client.GetAsync("");

            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task GetlAll_returnsContent()
        {
           var response = await _client.GetAsync("");

            Assert.NotNull(response.Content);
            Assert.True(response.Content.Headers.ContentLength > 0);
        }

        [Fact]
        public async Task GetAll_ReturnsExpectedJson()
            {
                var expected = new List<string> { "Accessories","Bags","Balls","Clothing","Rackets"};

                var responseStream = await _client.GetStreamAsync("");

                var model = await JsonSerializer.DeserializeAsync<ExpectedCategoriesModel>(responseStream,
                    JsonSerializerHelper.DefaultDeserializationOptions);

                Assert.NotNull(model?.AllowedCategories);
                Assert.Equal(expected.OrderBy(x => x),model.AllowedCategories.OrderBy(x => x));
            }
        
        [Fact]
        public async Task GeAll_ReturnsExpectedResponse()
        {
            //all
            
            var expected = new List<string> { "Accessories","Bags","Balls","Clothing","Rackets"};

            var model = await _client.GetFromJsonAsync<ExpectedCategoriesModel>("");

            Assert.NotNull(model?.AllowedCategories);
            Assert.Equal(expected.OrderBy(x => x), model.AllowedCategories.OrderBy(x => x));
   
        }

        [Fact]
        public async Task GetAll_SetsExpectedCacheControlHeader()
        {
            var response = await _client.GetAsync("");
            
            var header = response.Headers.CacheControl;

            Assert.True(header.MaxAge.HasValue);
            Assert.Equal(TimeSpan.FromMinutes(5), header.MaxAge);
            Assert.True(header.Public);
        }
    }
}