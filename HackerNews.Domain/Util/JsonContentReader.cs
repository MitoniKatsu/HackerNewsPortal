using System.Text.Json;

namespace HackerNews.Domain.Util
{
    public static class JsonContentReader
    {
        private static JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        public static async Task<T?> ReadJsonContent<T>(this HttpContent content)
        {
            var stringContent = await content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(stringContent, _options);
        }
    }
}
