using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TennisBookings.Api.IntegrationTests.Fakes;
using TennisBookings.Merchandise.Api.Diagnostics;
using TennisBookings.Merchandise.Api.External.Database;

namespace TennisBookings.Api.IntegrationTests
{
   public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where
        TStartup : class
    {
        public FakeCloudDatabase FakeCloudDatabase { get; }

        public CustomWebApplicationFactory()
        {
            FakeCloudDatabase = FakeCloudDatabase.WithDefaultProducts();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<ICloudDatabase>(FakeCloudDatabase);
                services.AddSingleton<IMetricRecorder>(new FakeMetricRecorder());
            });
        }
    }
}
