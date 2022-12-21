using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xacte.Common.Utils
{
    public static class JsonSerializerUtils
    {
        public static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public static string Serialize(object value, Type? type = null)
        {
            return type is null
                ? JsonSerializer.Serialize(value, GetJsonSerializerOptions())
                : JsonSerializer.Serialize(value, type, GetJsonSerializerOptions());
        }

        public static dynamic? Deserialize(string serialized, Type? type = null)
        {
            return type is null
                ? JsonSerializer.Deserialize<JsonElement>(serialized, GetJsonSerializerOptions())
                : (dynamic)(JsonSerializer.Deserialize(serialized, type, GetJsonSerializerOptions()) ?? new object());
        }

        public static T? Deserialize<T>(string serialized)
        {
            T? deserialized = JsonSerializer.Deserialize<T>(serialized, GetJsonSerializerOptions());
            return deserialized;
        }
    }
}
