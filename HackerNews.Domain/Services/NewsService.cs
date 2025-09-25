using HackerNews.Domain.Interfaces;
using HackerNews.Domain.Models;
using HackerNews.Domain.Models.DTO;
using HackerNews.Domain.Util;
using Microsoft.Extensions.Logging;
using System.Net;

namespace HackerNews.Domain.Services
{
    public class NewsService(ILogger<NewsService> logger, ICacheService cacheService, HttpClient client) : INewsService
    {
        private readonly ILogger<NewsService> _logger = logger;
        private readonly ICacheService _cacheService = cacheService;
        private readonly HttpClient _client = client;

        public async Task<PagedResponseDto<NewsStoryDto>> GetLatestStories(LatestNewsRequest request)
        {
            try
            {
                var cacheKey = _cacheService.Get<string>(Constants.CACHEKEY_GUID);
                var currentStories = _cacheService.Get<IList<NewsStoryDto>>(cacheKey ?? string.Empty);

                if (cacheKey == default)
                {
                    cacheKey = RefreshCacheKey();
                }

                if (currentStories == default)
                {
                    cacheKey = RefreshCacheKey();
                    var ids = await RefreshStoryIds() ?? [];
                    currentStories = await RefreshStories(ids, cacheKey);
                }

                var maxPageNumber = (int)Math.Floor((double)(currentStories.Count / request.PageSize) + 1);
                var pageNumber = request.PageNumber > maxPageNumber ? maxPageNumber : request.PageNumber;

                var pageStories = currentStories
                    .Skip((pageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();


                return new PagedResponseDto<NewsStoryDto>
                {
                    Total = currentStories.Count,
                    PageNumber = pageNumber,
                    PageCount = maxPageNumber,
                    Page = pageStories
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<PagedResponseDto<RankedNewsStoryDto>> GetSearchRankedStories(NewsSearchRequest request)
        {
            try
            {
                var cacheKey = _cacheService.Get<string>(Constants.CACHEKEY_GUID);
                var currentStories = _cacheService.Get<IList<NewsStoryDto>>(cacheKey ?? string.Empty);
                var currentRankedStories = _cacheService.Get<IList<RankedNewsStoryDto>>($"{cacheKey ?? string.Empty}_{request.SearchString}");

                if (cacheKey == default)
                {
                    cacheKey = RefreshCacheKey();
                }

                if (currentStories == default)
                {
                    cacheKey = RefreshCacheKey();
                    var ids = await RefreshStoryIds() ?? [];
                    currentStories = await RefreshStories(ids, cacheKey);
                }

                if (currentRankedStories == default)
                {
                    currentRankedStories = RankStoriesBySearch(currentStories, request.SearchString) ?? [];
                    _cacheService.Insert($"{cacheKey ?? string.Empty}_{request.SearchString}", currentRankedStories);
                }

                var maxPageNumber = (int)Math.Floor((double)(currentRankedStories.Count / request.PageSize) + 1);
                var pageNumber = request.PageNumber > maxPageNumber ? maxPageNumber : request.PageNumber;

                var pageStories = currentRankedStories
                    .OrderByDescending(o => o.SearchRanking)
                    .Skip((pageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();


                return new PagedResponseDto<RankedNewsStoryDto>
                {
                    Total = currentRankedStories.Count,
                    PageNumber = pageNumber,
                    PageCount = maxPageNumber,
                    Page = pageStories
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Ranks a list of news stories by the search string relevancy, and removes
        /// records that have no relevancy, returning the remaining
        /// </summary>
        /// <param name="unrankedStories">provided list of unranked new stories</param>
        /// <param name="searchString">provided search string</param>
        /// <returns>a list or relevant stories, sorted by relevance</returns>
        private IList<RankedNewsStoryDto> RankStoriesBySearch(IList<NewsStoryDto> unrankedStories, string searchString)
        {
            var searchTokens = searchString.Split(' ');
            var rankedStories = unrankedStories.Select(s =>
            {
                var rankedStory = new RankedNewsStoryDto(s);
                foreach (var token in searchTokens)
                {
                    if (rankedStory.Title.Contains(token, StringComparison.InvariantCultureIgnoreCase))
                        rankedStory.SearchRanking++;
                }
                return rankedStory;
            }).Where(o => o.SearchRanking > 0).ToList();

            return rankedStories;
        }

        /// <summary>
        /// refreshes the expired cache key guid stored in the cache
        /// </summary>
        /// <returns>the new cache key guid</returns>
        private string RefreshCacheKey()
        {
            var cacheKey = Guid.NewGuid().ToString();
            _cacheService.Insert(Constants.CACHEKEY_GUID, cacheKey);

            return cacheKey;
        }

        /// <summary>
        /// refreshes the current list of up to the top 500 newest stories from Hacker News API
        /// </summary>
        /// <returns>the list of Hacker News story ids</returns>
        private async Task<IList<int>?> RefreshStoryIds()
        {
            try
            {
                var response = await _client.GetAsync("newstories.json");

                if (response.StatusCode == HttpStatusCode.OK && response.Content != null)
                {
                    var storyIds = await response.Content.ReadJsonContent<IList<int>>();

                    return storyIds;
                }

                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError($"A problem occurred while refreshing story id list, {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Creates and runs a task to retrieve the news stories assocated with the supplied
        /// id list from the Hacker News API, filtering out nulls
        /// </summary>
        /// <param name="storyIds">the supplied list of story ids</param>
        /// <param name="cacheKey"> the current cache key guid</param>
        /// <returns>the list of stories belonging to the supplied id list</returns>
        private async Task<IList<NewsStoryDto>> RefreshStories(IList<int> storyIds, string cacheKey)
        {
            List<NewsStoryDto> stories = new();
            List<Task<NewsStoryDto?>> refreshTasks = new();

            foreach (var id in storyIds)
            {
                refreshTasks.Add(GetStory(id));
            }

            await Task.WhenAll(refreshTasks).ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully && t.Result != null)
                {
                    foreach (var story in t.Result)
                    {
                        if (story != default)
                        {
                            stories.Add(story);
                        }
                    }
                    _cacheService.Insert(Constants.CACHEKEY_GUID, cacheKey);
                    _cacheService.Insert(cacheKey, stories);
                }
            });

            return stories;
        }

        /// <summary>
        /// Gets the story associated with the supplied id from the Hacker News API, and only returns
        /// the <see cref="NewsStoryDto"/> if the story is not dead, the type is "story", and the URL
        /// is not empty
        /// </summary>
        /// <param name="storyId">supplied story id</param>
        /// <returns>the <see cref="NewsStoryDto"/> of the requested id</returns>
        private async Task<NewsStoryDto?> GetStory(int storyId)
        {
            try
            {
                var response = await _client.GetAsync($"item/{storyId}.json");
                if (response.StatusCode == HttpStatusCode.OK && response.Content != null)
                {
                    var story = await response.Content.ReadJsonContent<NewsStory>();
                    if (story != default && !story.Dead && story.Type == Constants.NEWSTYPE_STORY && !string.IsNullOrEmpty(story.Url))
                    {
                        return new NewsStoryDto
                        {
                            Id = storyId,
                            Title = story.Title,
                            Url = story.Url
                        };
                    }
                }

                return default;

            }
            catch (Exception ex)
            {
                _logger.LogError($"A problem occurred while refreshing story id list, {ex.Message}");
                throw;
            }
        }
    }
}
