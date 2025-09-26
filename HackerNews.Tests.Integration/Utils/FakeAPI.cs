using HackerNews.Domain;
using HackerNews.Domain.Models;
using System.Net;
using System.Text.Json;

namespace HackerNews.Tests.Integration.Utils
{
    public static class FakeAPI
    {
        // Ids
        public readonly static IList<int> ID_LIST_1 = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        public readonly static IList<int> ID_LIST_2 = [11, 12, 13, 14, 15, 16, 17, 18, 19, 20];

        // Successfuly Id Requests
        public readonly static HttpResponseMessage ID_RESPONSE_1 = new HttpResponseMessage
        {
            Content = new StringContent(JsonSerializer.Serialize(ID_LIST_1)),
            StatusCode = HttpStatusCode.OK
        };
        public readonly static HttpResponseMessage ID_RESPONSE_2 = new HttpResponseMessage
        {
            Content = new StringContent(JsonSerializer.Serialize(ID_LIST_2)),
            StatusCode = HttpStatusCode.OK
        };

        // Successful Story Requests
        public static IList<int> STORY_RESPONSE_1_OUTLIERS = [3, 5, 7];
        public readonly static IList<HttpResponseMessage> STORY_RESPONSES_1 = ID_LIST_1.Select(id =>
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(JsonSerializer.Serialize(new NewsStory
                {
                    Id = id,
                    Type = !(id == STORY_RESPONSE_1_OUTLIERS[0]) ? Constants.NEWSTYPE_STORY : "TEST",
                    Dead = !(id == STORY_RESPONSE_1_OUTLIERS[1]) ? false : true,
                    Url = !(id == STORY_RESPONSE_1_OUTLIERS[2]) ? $"url{id}" : null,
                    Title = $"title{id} test"
                }))
            };
        }).ToList();

        public static IList<int> STORY_RESPONSE_2_OUTLIERS = [13, 15, 17];
        public readonly static IList<HttpResponseMessage> STORY_RESPONSES_2 = ID_LIST_2.Select(id =>
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(JsonSerializer.Serialize(new NewsStory
                {
                    Id = id,
                    Type = !(id == STORY_RESPONSE_2_OUTLIERS[0]) ? Constants.NEWSTYPE_STORY : "TEST",
                    Dead = !(id == STORY_RESPONSE_2_OUTLIERS[1]) ? false : true,
                    Url = !(id == STORY_RESPONSE_2_OUTLIERS[2]) ? $"url{id}" : null,
                    Title = $"title{id}"
                }))
            };
        }).ToList();
    }
}
