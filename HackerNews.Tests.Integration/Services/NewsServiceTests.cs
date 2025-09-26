using HackerNews.Domain.Models.DTO;
using HackerNews.Domain.Util;
using HackerNews.Server;
using HackerNews.Tests.Integration.Utils;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using Shouldly;
using System.Net;
using System.Text.Encodings.Web;

namespace HackerNews.Tests.Integration.Services
{
    public class NewsServiceTests : IClassFixture<IntegrationTestingPlatform<Program>>
    {
        private readonly IConfiguration? _configuration;
        private readonly IntegrationTestingPlatform<Program>? _factory;

        public NewsServiceTests(IntegrationTestingPlatform<Program> factory)
        {
            _configuration = factory.Services.GetService(typeof(IConfiguration)) as IConfiguration;
            _factory = factory;
        }

        [Fact]
        public async Task GetLatestStories_WhenLatestStoriesRequested_ShouldCacheAndReturnLatestStories()
        {
            var expectedResultIds = FakeAPI.ID_LIST_1.Where(o => !FakeAPI.STORY_RESPONSE_1_OUTLIERS.Any(a => a == o));

            var storyResponses = FakeAPI.STORY_RESPONSES_1;
            _factory.MessageHandler?
                .Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(FakeAPI.ID_RESPONSE_1)
                .ReturnsAsync(storyResponses[0])
                .ReturnsAsync(storyResponses[1])
                .ReturnsAsync(storyResponses[2])
                .ReturnsAsync(storyResponses[3])
                .ReturnsAsync(storyResponses[4])
                .ReturnsAsync(storyResponses[5])
                .ReturnsAsync(storyResponses[6])
                .ReturnsAsync(storyResponses[7])
                .ReturnsAsync(storyResponses[8])
                .ReturnsAsync(storyResponses[9]);

            var client = _factory.CreateClient();

            var result = await client.GetAsync("/api/news");
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
            var content = await result.Content.ReadJsonContent<PagedResponseDto<NewsStoryDto>>();
            content.Page.Count.ShouldBe(expectedResultIds.Count());
            foreach (var story in content.Page)
            {
                expectedResultIds.Any(o => o == story.Id).ShouldBeTrue();
            }
        }

        [Fact]
        public async Task GetSearchRankedStories_WhenSearchRankedStoriesRequested_ShouldCacheAndReturnSearchRankedStories()
        {
            var expectedResultIds = FakeAPI.ID_LIST_1.Where(o => !FakeAPI.STORY_RESPONSE_1_OUTLIERS.Any(a => a == o));

            var storyResponses = FakeAPI.STORY_RESPONSES_1;
            _factory.MessageHandler?
                .Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(FakeAPI.ID_RESPONSE_1)
                .ReturnsAsync(storyResponses[0])
                .ReturnsAsync(storyResponses[1])
                .ReturnsAsync(storyResponses[2])
                .ReturnsAsync(storyResponses[3])
                .ReturnsAsync(storyResponses[4])
                .ReturnsAsync(storyResponses[5])
                .ReturnsAsync(storyResponses[6])
                .ReturnsAsync(storyResponses[7])
                .ReturnsAsync(storyResponses[8])
                .ReturnsAsync(storyResponses[9]);

            var client = _factory.CreateClient();

            var searchString = "1 test";
            var result = await client.GetAsync($"/api/news/search?searchString={searchString}");
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
            var content = await result.Content.ReadJsonContent<PagedResponseDto<RankedNewsStoryDto>>();
            content.Page.Count.ShouldBe(expectedResultIds.Count());
            foreach (var story in content.Page)
            {
                expectedResultIds.Any(o => o == story.Id).ShouldBeTrue();
            }
            content.Page.Count(o => o.SearchRanking > 1).ShouldBe(2);
            // top ranking searches are higher on list
            content.Page.ToList()[0].SearchRanking.ShouldBe(2);
            content.Page.ToList()[1].SearchRanking.ShouldBe(2);
            for (int i = 2; i < content.Page.Count; i++)
            {
                content.Page.ToList()[i].SearchRanking.ShouldBe(1);
            }
        }
    }
}
