using HackerNews.Domain.Models.Options;
using HackerNews.Domain.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;

namespace HackerNews.Tests.Core.Services
{
    public class CacheServiceTests
    {
        private readonly IMemoryCache _cache = Substitute.For<IMemoryCache>();
        private readonly IOptions<CacheOptions> _options = Substitute.For<IOptions<CacheOptions>>();

        [Fact]
        public void Get_WhenCacheHasRecord_ReturnsRecord()
        {
            var key = "test";
            var val = "value";

            _cache.TryGetValue(Arg.Is(key), out Arg.Any<string?>()).Returns(x =>
            {
                x[1] = val;
                return true;
            });

            var service = new CacheService(_cache, _options);

            service.Get<string>(key).ShouldBe(val);
        }

        [Fact]
        public void Get_WhenCacheDoesntHasRecord_ReturnsDefault()
        {
            var key = "test";

            _cache.TryGetValue(Arg.Is(key), out Arg.Any<string?>()).Returns(x =>
            {
                return true;
            });

            var service = new CacheService(_cache, _options);

            service.Get<string>(key).ShouldBe(default);
        }

        [Fact]
        public void Insert_ShouldCallCacheSetWithOptions()
        {
            var key = "test";
            var val = "value";
            var sldExp = 10;
            var absExp = 30;

            var entryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(sldExp))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(absExp));

            _options.Value.Returns(new CacheOptions
            {
                SlidingExpirationSeconds = sldExp,
                AbsoluteExpirationSeconds = absExp
            });

            var service = new CacheService(_cache, _options);

            service.Insert(key, val);

            _cache.Received(1).Set(key, val, entryOptions);
        }
    }
}
