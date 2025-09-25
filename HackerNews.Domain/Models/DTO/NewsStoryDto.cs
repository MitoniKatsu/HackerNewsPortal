namespace HackerNews.Domain.Models.DTO
{
    public class NewsStoryDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }
}
