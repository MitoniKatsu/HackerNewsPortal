namespace HackerNews.Domain.Models.Options
{
    public class CacheOptions
    {
        public const string Cache = "Cache";
        public int SlidingExpirationSeconds { get; set; }
        public int AbsoluteExpirationSeconds { get; set; }
    }
}
