namespace HackerNews.Domain.Interfaces
{
    public interface ICacheService
    {
        /// <summary>
        /// Get a record from the Cache
        /// </summary>
        /// <typeparam name="T">Record Type</typeparam>
        /// <param name="key">Cache key</param>
        /// <returns>The retrieved record or default</returns>
        T? Get<T>(string key);

        /// <summary>
        /// Insert a record into the Cache with a specific key, absolute expiration, and
        /// an optional sliding expiration window
        /// </summary>
        /// <typeparam name="T">Record Type</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="newCacheRecord">Record to insert into cache</param>
        /// <param name="slidingExpiration">bool if sliding expiration is to be applied, (default is true)</param>
        void Insert<T>(string key, T newCacheRecord, bool slidingExpiration = true);
    }
}