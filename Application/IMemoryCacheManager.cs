using System.Collections.Concurrent;

namespace Application
{
    /// <summary>
    /// Cache manager
    /// </summary>
    public interface IMemoryCacheManager
    {
        /// <summary>
        /// Get cache from queue
        /// </summary>
        /// <returns></returns>
        public Task<ConcurrentQueue<object>> GetAsync();

        /// <summary>
        /// Add cache object into queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task AddAsync<T>(T value);

        /// <summary>
        /// Delete cache from queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync<T>(out T value);

        /// <summary>
        /// Clear all cache objects from queue
        /// </summary>
        /// <returns></returns>
        public Task<bool> ClearAllAsync();
    }
}
