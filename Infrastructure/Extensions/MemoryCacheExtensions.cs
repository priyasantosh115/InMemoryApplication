using Application;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    /// <summary>
    /// DI for Memory cache
    /// </summary>
    public static class MemoryCacheExtensions
    {
        /// <summary>
        /// Add singleton dependencies for Memory cache
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddMemoryCache(this IServiceCollection services, MemoryCacheSettings config)
        {
            services.AddSingleton(config);
            services.AddSingleton<IMemoryCacheManager, MemoryCacheManager>();
            return services;
        }
    }
}
