using HackerNews.Domain;
using HackerNews.Domain.Interfaces;
using HackerNews.Domain.Models;
using HackerNews.Domain.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;

namespace HackerNews.Tests.Core.Services
{
    public class NewsServiceTests
    {
        private readonly ILogger<NewsService> _logger = Substitute.For<ILogger<NewsService>>();
        private readonly ICacheService _cacheService = Substitute.For<ICacheService>();
        private readonly HttpClient _client = Substitute.For<HttpClient>();


        [Fact]
        public async Task GetLatestStories_WhenAnExceptionIsThrown_ShouldLogAndThrow()
        {
            var ex = new Exception("testing");
            _cacheService.Get<string>(Constants.CACHEKEY_GUID).Throws(new Exception("testing"));
            var service = new NewsService(_logger, _cacheService, _client);

            var thrown = await Should.ThrowAsync<Exception>(() => service.GetLatestStories(new LatestNewsRequest()));
            thrown.Message.ShouldBe(ex.Message);
            _logger.Received(1).LogError(ex.Message);
        }

        [Fact]
        public async Task GetSearchRankedStories_WhenAnExceptionIsThrown_ShouldLogAndThrow()
        {
            var ex = new Exception("testing");
            _cacheService.Get<string>(Constants.CACHEKEY_GUID).Throws(new Exception("testing"));
            var service = new NewsService(_logger, _cacheService, _client);

            var thrown = await Should.ThrowAsync<Exception>(() => service.GetSearchRankedStories(new NewsSearchRequest()));
            thrown.Message.ShouldBe(ex.Message);
            _logger.Received(1).LogError(ex.Message);
        }
    }
}
