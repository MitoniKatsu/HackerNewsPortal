using HackerNews.Domain.Interfaces;
using HackerNews.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Server.Controllers
{
    /// <summary>
    /// Handles requests related to Hacker News
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController(INewsService newsService) : ControllerBase
    {
        private readonly INewsService _newsService = newsService;

        /// <summary>
        /// Gets the latest stories from hacker news
        /// </summary>
        /// <returns>A paginated list of news stories</returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        public async Task<IActionResult> GetLatestStories([FromQuery] LatestNewsRequest request)
        {
            var result = await _newsService.GetLatestStories(request);
            if (result?.Page?.Count < 1)
            {
                return NoContent();
            }
            return Ok(result);
        }

        /// <summary>
        /// Gets the latest stories from hacker news that match the supplied search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>A paginated list of relevant news stories</returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        [Route("search")]
        public IActionResult SearchNewStories([FromQuery] NewsSearchRequest request)
        {
            throw new NotImplementedException($"search for {request.SearchString}");
        }
    }
}
