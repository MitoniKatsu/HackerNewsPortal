using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
