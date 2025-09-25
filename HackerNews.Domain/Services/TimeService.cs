using HackerNews.Domain.Interfaces;

namespace HackerNews.Domain.Services
{
    public class TimeService : ITimeService
    {
        public static DateTime NowUTC()
        {
            return DateTime.UtcNow;
        }
    }
}
