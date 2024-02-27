using Application;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Infrastructure
{
    /// <summary>
    /// Cache manager
    /// </summary>
    public class MemoryCacheManager : IMemoryCacheManager
    {
        private readonly MemoryCacheSettings _memoryCacheSettings;
        private readonly ILogger<MemoryCacheManager> _logger;
        private readonly ConcurrentQueue<object> _memoryCacheQueue;
        private readonly object _lock = new();

        public MemoryCacheManager(MemoryCacheSettings config, ILogger<MemoryCacheManager> logger)
        {
            _memoryCacheSettings = config;
            _logger = logger;
            _memoryCacheQueue = new ConcurrentQueue<object>();
        }

        /// <summary>
        /// Get cache from queue
        /// </summary>
        /// <returns></returns>
        public Task<ConcurrentQueue<object>> GetAsync()
        {
            if (!_memoryCacheQueue.IsEmpty)
            {
                _logger.LogInformation($"Data found in cache.");

                Console.WriteLine("\nData from cache:");

                int count = 1;
                foreach (var objectValue in _memoryCacheQueue)
                {
                    Console.WriteLine($"{count}. {objectValue}");
                    count++;
                }
                
                Console.WriteLine("\n");
                
                return Task.FromResult(_memoryCacheQueue);
            }
            _logger.LogInformation($"No data found in cache.");
            return Task.FromResult(_memoryCacheQueue);
        }

        /// <summary>
        /// Add cache object into queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task AddAsync<T>(T value)
        {
            lock (_lock)
            {
                if (_memoryCacheQueue.Count >= _memoryCacheSettings.MemoryCacheObjectThreshold && !_memoryCacheQueue.IsEmpty)
                {
                    _logger.LogInformation($"Cache capacity reached.");
                    _memoryCacheQueue.TryDequeue(out var oldValue);
                    _logger.LogInformation($"Oldest value '{oldValue}' removed from the cache.");
                }
                _memoryCacheQueue.Enqueue(value);
                _logger.LogInformation($"New value '{value}' added to the cache.");
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Delete cache from queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync<T>(out T value)
        {
            value = default;
            if (!_memoryCacheQueue.IsEmpty)
            {
                _memoryCacheQueue.TryDequeue(out var oldValue);
                _logger.LogInformation($"Data '{oldValue}' deleted successfully from cache.");
                return Task.FromResult(true);
            }
            _logger.LogInformation($"No data found in cache.");
            return Task.FromResult(false);
        }

        /// <summary>
        /// Clear all cache objects from queue
        /// </summary>
        /// <returns></returns>
        public Task<bool> ClearAllAsync()
        {
            if (!_memoryCacheQueue.IsEmpty)
            {
                _memoryCacheQueue.Clear();
                _logger.LogInformation($"Cache cleared successfully.");
                return Task.FromResult(true);
            }
            _logger.LogInformation($"No data found in cache.");
            return Task.FromResult(false);
        }
    }
}
