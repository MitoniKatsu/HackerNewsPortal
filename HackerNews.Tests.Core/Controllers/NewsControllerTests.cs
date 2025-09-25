using HackerNews.Domain.Interfaces;
using HackerNews.Domain.Models;
using HackerNews.Domain.Models.DTO;
using HackerNews.Server.Controllers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using System.Net;

namespace HackerNews.Tests.Core.Controllers
{
    public class NewsControllerTests
    {
        private readonly INewsService _newsService = Substitute.For<INewsService>();

        [Fact]
        public async Task GetLatestStories_WhenRequestProvided_ShouldCallGetLatestStoriesWithRequest()
        {
            LatestNewsRequest request = new LatestNewsRequest
            {
                PageNumber = 2,
                PageSize = 15
            };

            var controller = new NewsController(_newsService);

            await controller.GetLatestStories(request);

            await _newsService.Received(1).GetLatestStories(request);
        }

        [Fact]
        public async Task SearchNewStories_WhenRequestProvided_ShouldCallGetSearchRankedStoriesWithRequest()
        {
            NewsSearchRequest request = new NewsSearchRequest
            {
                PageNumber = 2,
                PageSize = 15,
                SearchString = "test"
            };

            var controller = new NewsController(_newsService);

            await controller.SearchNewStories(request);

            await _newsService.Received(1).GetSearchRankedStories(request);
        }

        [Fact]
        public async Task GetLatestStories_WhenResultReturned_ShouldReturnOkWithResult()
        {
            LatestNewsRequest request = new LatestNewsRequest();

            var expected = new PagedResponseDto<NewsStoryDto>
            {
                PageNumber = 1,
                PageCount = 1,
                Page = new List<NewsStoryDto>
                {
                    new()
                    {
                        Title = "Test",
                        Url = "www.test.com"
                    }
                }
            };

            _newsService.GetLatestStories(request).Returns(expected);

            var controller = new NewsController(_newsService);

            IActionResult result = await controller.GetLatestStories(request);

            var okResult = result as OkObjectResult;

            okResult?.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            okResult?.Value.ShouldBe(expected);
        }

        [Fact]
        public async Task GetLatestStories_WhenNoResultReturned_ShouldReturnNoContent()
        {
            LatestNewsRequest request = new LatestNewsRequest();

            var expected = new PagedResponseDto<NewsStoryDto>
            {
                PageNumber = 1,
                PageCount = 1,
                Page = new List<NewsStoryDto>()
            };

            _newsService.GetLatestStories(request).Returns(expected);

            var controller = new NewsController(_newsService);

            IActionResult result = await controller.GetLatestStories(request);

            var noContentResult = result as NoContent;

            noContentResult?.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task SearchNewStories_WhenResultReturned_ShouldReturnOkWithResult()
        {
            NewsSearchRequest request = new NewsSearchRequest();

            var expected = new PagedResponseDto<RankedNewsStoryDto>
            {
                PageNumber = 1,
                PageCount = 1,
                Page = new List<RankedNewsStoryDto>
                {
                    new()
                    {
                        Id = 1,
                        Title = "Test",
                        Url = "www.test.com",
                        SearchRanking = 1
                    }
                }
            };

            _newsService.GetSearchRankedStories(request).Returns(expected);

            var controller = new NewsController(_newsService);

            IActionResult result = await controller.SearchNewStories(request);

            var okResult = result as OkObjectResult;

            okResult?.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            okResult?.Value.ShouldBe(expected);
        }

        [Fact]
        public async Task SearchNewStories_WhenNoResultReturned_ShouldReturnNoContent()
        {
            NewsSearchRequest request = new NewsSearchRequest();

            var expected = new PagedResponseDto<RankedNewsStoryDto>
            {
                PageNumber = 1,
                PageCount = 1,
                Page = new List<RankedNewsStoryDto>()
            };

            _newsService.GetSearchRankedStories(request).Returns(expected);

            var controller = new NewsController(_newsService);

            IActionResult result = await controller.SearchNewStories(request);

            var noContentResult = result as NoContent;

            noContentResult?.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
        }
    }
}
