using Application;
using Domain.Entities;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

var cache = new ServiceCollection()
    .AddLogging(builder => builder.AddSerilog(dispose: true))
    .AddMemoryCache(new MemoryCacheSettings { MemoryCacheObjectThreshold = 5 })
    .BuildServiceProvider()
    .GetService<IMemoryCacheManager>();

await cache.AddAsync("Data 1");

await cache.AddAsync(90);

await cache.GetAsync();

await cache.AddAsync("Data 2");
await cache.AddAsync("Data 3");
await cache.AddAsync("Data 4");
await cache.AddAsync("Data 5");

await cache.GetAsync();

await cache.AddAsync("Data 6");
await cache.AddAsync("Data 7");
await cache.AddAsync("Data 8");
await cache.AddAsync("Data 9");

await cache.DeleteAsync(out string deletedValue);

await cache.GetAsync();

await cache.ClearAllAsync();

await cache.GetAsync();
