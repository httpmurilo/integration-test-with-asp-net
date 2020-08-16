using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TennisBookings.Api.IntegrationTests.Model;
using TennisBookings.Api.IntegrationTests.TestHelpers;
using TennisBookings.Merchandise.Api;
using TennisBookings.Merchandise.Api.Data;
using Xunit;

namespace TennisBookings.Api.IntegrationTests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public ProductsControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/products/");
            _client = factory.CreateClient();
            _factory = factory;
        }

        [Fact]
        public async Task GetAll_ReturnsExpectedArrayOfProducts()
        {
            _factory.FakeCloudDatabase.ResetDefaultProducts(useCustomIfAvailable: false);

            var products = await _client.GetFromJsonAsync<ExpectedProductModel[]>("");

            Assert.NotNull(products);
            Assert.Equal(_factory.FakeCloudDatabase.Products.Count, products.Count());
        }

        [Fact]
        public async Task Get_ReturnsExpectedProduct()
        {
            var firstProduct = _factory.FakeCloudDatabase.Products.First();

            var product = await _client.GetFromJsonAsync<ExpectedProductModel>($"{firstProduct.Id}");

            Assert.NotNull(product);
            Assert.Equal(firstProduct.Name, product.Name);
        }

        [Fact]
        public async Task Post_WithoutName_ReturnsBadRequest()
        {
            var productInputModel = GetValidProductInputModel().CloneWith( m => m.Name = null);

            var response = await _client.PostAsJsonAsync("", productInputModel, JsonSerializerHelper.DefaultDeserializationOptions);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private static TestProductInputModel GetValidProductInputModel(Guid? id = null)
        {
            return new TestProductInputModel
            {
                Id = id is object ? id.Value.ToString() : Guid.NewGuid().ToString(),
                Name = "Some product",
                Description = "This a description",
                Category = new CategoryProvider().AllowedCategories().First(),
                InternalReference = "ABC123",
                Price = 4.00m
            };
        }

    }
}
