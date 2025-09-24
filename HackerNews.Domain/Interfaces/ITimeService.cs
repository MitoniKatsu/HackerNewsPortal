namespace HackerNews.Domain.Interfaces
{
    public interface ITimeService
    {
        static abstract DateTime NowUTC();
    }
}