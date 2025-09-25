using HackerNews.Domain.Interfaces;
using HackerNews.Domain.Models.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace HackerNews.Domain.Services
{
    public class CacheService(IMemoryCache cache, IOptions<CacheOptions> options) : ICacheService
    {
        private readonly IMemoryCache _cache = cache;
        private readonly int _slidingExp = options?.Value?.SlidingExpirationSeconds ?? 300;
        private readonly int _absoluteExp = options?.Value?.AbsoluteExpirationSeconds ?? 3600;

        public void Insert<T>(string key, T newCacheRecord)
        {
            var entryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(_slidingExp))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(_absoluteExp));
            _cache.Set(key, newCacheRecord, entryOptions);
        }

        public T? Get<T>(string key)
        {
            if (_cache.TryGetValue(key, out T? value))
            {
                return value;
            }
            return default;
        }
    }
}
