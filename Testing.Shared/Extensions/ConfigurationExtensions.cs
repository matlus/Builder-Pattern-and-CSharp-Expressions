using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json;

namespace Testing.Shared
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddAsJsonRoot(this IConfigurationBuilder builder, object configSettingsRoot)
        {
            var memoryStream = new MemoryStream();
            SerializeToJson(memoryStream, configSettingsRoot);
            memoryStream.Position = 0;
            return builder.AddJsonStream(memoryStream);
        }

        private static void SerializeToJson(Stream stream, object value)
        {
            using var utf8JsonWriter = new Utf8JsonWriter(stream);
            JsonSerializer.Serialize(utf8JsonWriter, value);
            utf8JsonWriter.Flush();
        }
    }
}
