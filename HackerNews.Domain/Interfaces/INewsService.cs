using HackerNews.Domain.Models;
using HackerNews.Domain.Models.DTO;

namespace HackerNews.Domain.Interfaces
{
    public interface INewsService
    {
        /// <summary>
        /// Gets a paginated list of the newest news
        /// </summary>
        /// <param name="request">a request object implementing <see cref="PagedRequestBase"/></param>
        /// <returns>a <see cref="PagedResponseDto{T}"/> of type <see cref="NewsStoryDto"/></returns>
        Task<PagedResponseDto<NewsStoryDto>> GetLatestStories(LatestNewsRequest request);

        /// <summary>
        /// Gets a paginated list of the newest news relevant to the provided search string 
        /// </summary>
        /// <param name="request">a request object implementing <see cref="PagedRequestBase"/> with the addition of a search string</param>
        /// <returns>a <see cref="PagedResponseDto{T}"/> of type <see cref="RankedNewsStoryDto"/></returns>
        Task<PagedResponseDto<RankedNewsStoryDto>> GetSearchRankedStories(NewsSearchRequest request);
    }
}