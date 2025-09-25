namespace HackerNews.Domain.Models
{
    public class NewsSearchRequest : PagedRequestBase
    {
        public string SearchString { get; set; } = string.Empty;
    }
}
