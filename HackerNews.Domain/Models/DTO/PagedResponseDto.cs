namespace HackerNews.Domain.Models.DTO
{
    public class PagedResponseDto<T>
    {
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public ICollection<T> Page { get; set; } = [];
    }
}
