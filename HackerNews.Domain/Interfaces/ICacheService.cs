namespace HackerNews.Domain.Interfaces
{
    public interface ICacheService
    {
        T? Get<T>(string key);
        void Insert<T>(string key, T newCacheRecord);
    }
}