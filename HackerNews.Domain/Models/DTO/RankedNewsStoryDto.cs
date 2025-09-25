using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Domain.Models.DTO
{
    public class RankedNewsStoryDto : NewsStoryDto
    {
        public RankedNewsStoryDto(NewsStoryDto newsStory)
        {
            Id = newsStory.Id;
            Url = newsStory.Url;
            Title = newsStory.Title;
        }

        public int SearchRanking { get; set; }
    }
}
