using HackerNews.Domain.Interfaces;
using HackerNews.Domain.Models;
using HackerNews.Domain.Models.DTO;
using HackerNews.Domain.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HackerNews.Domain.Services
{
    public class NewsService(ILogger<NewsService> logger, HttpClient client) : INewsService
    {
        private readonly ILogger<NewsService> _logger = logger;
        private readonly HttpClient _client = client;

        private IList<int> tempIdCache = new List<int>();
        private IList<NewsStoryDto> tempCache = new List<NewsStoryDto>();
        public async Task<PagedResponseDto<NewsStoryDto>> GetLatestStories(LatestNewsRequest request)
        {
            await RefreshLatestStoryIds();
            await RefreshLatestStories();
            return new PagedResponseDto<NewsStoryDto>
            {
                Total = tempCache.Count,
                PageNumber = request.PageNumber,
                Page = tempCache.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList()
            };
        }

        private async Task RefreshLatestStoryIds()
        {
            try
            {
                var response = await _client.GetAsync("newstories.json");
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    tempIdCache = JsonSerializer.Deserialize<IList<int>>(content) ?? [];
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private async Task RefreshLatestStories()
        {
            try
            {
                var freshStoryCache = new List<NewsStoryDto>();
                var tasks = new List<Task<NewsStoryDto>>();


                await Task.WhenAll(tempIdCache.Select(o => GetStory(o))).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        _logger.LogError($"Failed to retrieve story: {t.Exception?.Message}");
                        return;
                    }

                    if (t.IsCompletedSuccessfully && t.Result != null)
                    {
                        freshStoryCache = t.Result.Where(o => o != null).ToList();
                    }
                });

                tempCache = freshStoryCache;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private async Task<NewsStoryDto> GetStory(int storyId)
        {
            try
            {
                var response = await _client.GetAsync($"item/{storyId}.json");
                if (response.Content != null)
                {
                    var story = await response.Content.ReadJsonContent<NewsStory>();
                    if (story != null && !story.Dead && story.Type == Constants.NEWSTYPE_STORY && !string.IsNullOrEmpty(story.Url))
                    {
                        return new NewsStoryDto
                        {
                            Id = storyId,
                            Title = story.Title,
                            Url = story.Url
                        };
                    }
                }

                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
