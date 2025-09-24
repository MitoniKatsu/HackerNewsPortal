using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Domain.Models
{
    public class NewsStory
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public bool Dead { get; set; } = false;
        public string? Url { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
