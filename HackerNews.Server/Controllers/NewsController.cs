using HackerNews.Domain.Interfaces;
using HackerNews.Domain.Models;
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
        /// Gets a paginated list of the latest stories from hacker news 
        /// </summary>
        /// <param name="request"> a <see cref="LatestNewsRequest"/></param>
        /// <returns>A paginated list of news stories</returns>
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
        /// Gets a paginated list of the latest stories from hacker news that match the supplied search request
        /// </summary>
        /// <param name="request"> a <see cref="NewsSearchRequest"/></param>
        /// <returns>A paginated list of relevant news stories</returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchNewStories([FromQuery] NewsSearchRequest request)
        {
            var result = await _newsService.GetSearchRankedStories(request);
            if (result?.Page?.Count < 1)
            {
                return NoContent();
            }
            return Ok(result);
        }
    }
}
