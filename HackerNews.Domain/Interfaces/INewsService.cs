using HackerNews.Domain.Models;
using HackerNews.Domain.Models.DTO;

namespace HackerNews.Domain.Interfaces
{
    public interface INewsService
    {
        Task<PagedResponseDto<NewsStoryDto>> GetLatestStories(LatestNewsRequest request);
    }
}