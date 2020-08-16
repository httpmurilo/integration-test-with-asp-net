using System.Text.Json;

namespace TennisBookings.Api.IntegrationTests.TestHelpers
{
    public static class JsonSerializerHelper
    {
        public static JsonSerializerOptions DefaultSerialisaOptions => new JsonSerializerOptions
        {
            IgnoreNullValues = true
        };

        public static JsonSerializerOptions DefaultDeserializationOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
}