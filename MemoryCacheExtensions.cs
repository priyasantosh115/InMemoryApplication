public static class MemoryCacheExtensions
{
	public static IServiceCollection AddMemoryCache(this IServiceCollection services, MemoryCacheConfig config)
	{
		services.AddSingleton(config);
		services.AddSingleton<IGenericCache, MemoryCacheCore>();
		return services;
	}
}