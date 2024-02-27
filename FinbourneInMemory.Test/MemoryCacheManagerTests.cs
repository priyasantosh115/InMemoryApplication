using Domain.Entities;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;

namespace FinbourneInMemory.Test
{
    [TestClass]
    public class MemoryCacheManagerTests
    {
        [TestMethod]
        public void AddAsync_ShouldAddObjectToCache()
        {
            // Arrange
            var logger = new Mock<ILogger<MemoryCacheManager>>().Object;
            var cacheConfig = GetCacheConfig();
            var cache = new MemoryCacheManager(cacheConfig, logger);

            // Act
            cache.AddAsync("Value 1").Wait();

            // Assert
            Assert.AreEqual(1, cache.GetAsync().Result.Count);
        }

        [TestMethod]
        public void AddAsync_ShouldDeleteOldestObjectWhenCacheFull()
        {
            // Arrange
            var logger = new Mock<ILogger<MemoryCacheManager>>().Object;
            var cacheConfig = GetCacheConfig();
            var cache = new MemoryCacheManager(cacheConfig, logger);

            // Act
            cache.AddAsync("Value 1").Wait();
            cache.AddAsync("Value 2").Wait();
            cache.AddAsync("Value 3").Wait();
            cache.AddAsync("Value 4").Wait();

            var cacheResult = cache.GetAsync().Result;

            // Assert
            Assert.IsFalse(cacheResult.Contains("Value 1"));
            Assert.AreEqual(3, cacheResult.Count);
        }


        [TestMethod]
        public void AddAsync_ShouldDeleteOldestCacheWhenCacheFull()
        {
            // Arrange
            var logger = new Mock<ILogger<MemoryCacheManager>>().Object;
            var cacheConfig = GetCacheConfig();
            var cache = new MemoryCacheManager(cacheConfig, logger);

            // Act
            cache.AddAsync("Value 1").Wait();
            cache.AddAsync("Value 2").Wait();
            cache.AddAsync("Value 3").Wait();
            cache.GetAsync().Wait();
            cache.AddAsync("Value 4").Wait();

            var cacheResult = cache.GetAsync().Result;

            // Assert
            Assert.IsFalse(cacheResult.Contains("Value 1"));
            Assert.AreEqual(3, cacheResult.Count);
        }

        [TestMethod]
        public void DeleteAsync_ShouldDeleteObjectFromCache()
        {
            // Arrange
            var logger = new Mock<ILogger<MemoryCacheManager>>().Object;
            var cacheConfig = GetCacheConfig();
            var cache = new MemoryCacheManager(cacheConfig, logger);
            
            cache.AddAsync("Value 1").Wait();
            cache.AddAsync("Value 2").Wait();
            cache.AddAsync("Value 3").Wait();

            // Act
            cache.DeleteAsync(out string value).Wait();

            var cacheResult = cache.GetAsync().Result;

            // Assert
            Assert.IsFalse(cacheResult.Contains(value));
            Assert.AreEqual(2, cacheResult.Count);
        }

        [TestMethod]
        public void ClearAllAsync_ShouldClearAllCache()
        {
            // Arrange
            var logger = new Mock<ILogger<MemoryCacheManager>>().Object;
            var cacheConfig = GetCacheConfig();
            var cache = new MemoryCacheManager(cacheConfig, logger);
            cache.AddAsync("Value 1").Wait();
            cache.AddAsync("Value 2").Wait();

            // Act
            cache.ClearAllAsync().Wait();

            // Assert
            Assert.AreEqual(0, cache.GetAsync().Result.Count);
        }

        private MemoryCacheSettings GetCacheConfig()
        {
            return new MemoryCacheSettings { MemoryCacheObjectThreshold = 3 };
        }
    }
}
