using System.Text.Json;

namespace ComplianceAPI.Helpers
{
    public static class JsonHelper
    {
        public static string SerializeToJson<T>(T obj, JsonSerializerOptions options = null)
        {
            if (options == null)
            {
                options = new JsonSerializerOptions
                {
                    WriteIndented = true, // For pretty printing the JSON
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Optional: to use camelCase naming
                };
            }

            return JsonSerializer.Serialize(obj, options);
        }
        public static T DeserializeFromJson<T>(string jsonString, JsonSerializerOptions options = null)
        {
            if (options == null)
            {
                options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // To handle case insensitivity
                };
            }

            return JsonSerializer.Deserialize<T>(jsonString, options);
        }
    }
}
