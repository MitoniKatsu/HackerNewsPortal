using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Server.Controllers
{
    /// <summary>
    /// Handles requests related to Hacker News
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        /// <summary>
        /// Gets the latest stories from hacker news
        /// </summary>
        /// <returns>A paginated list of news stories</returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        public IActionResult GetLatestStories()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the latest stories from hacker news that match the supplied search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>A paginated list of relevant news stories</returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        [Route("search")]
        public IActionResult SearchNewStories([FromQuery] string searchString)
        {
            throw new NotImplementedException($"search for {searchString}");
        }
    }
}
