using Learning.Business.Contracts.Persistence;
using Microsoft.Extensions.Caching.Memory;

namespace Learning.Web.Impl.Persistence;

public class IAppMemoryCache : IAppCache
{
    private readonly IMemoryCache _memoryCache;

    public IAppMemoryCache(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public (bool HasData, T? Data) Get<T>(string key)
    {
        if (!_memoryCache.TryGetValue(key, out T value))
        {
            return (false, default);
        }
        else
        {
            return (true, value);
        }
    }

    public T Set<T>(string key, T Data)
    {
        _memoryCache.Set(key, Data);
        return Data;
    }

    public T SetWithSlidingExpiration<T>(string key, T Data, TimeSpan expiry)
    {
        MemoryCacheEntryOptions slidingExpiration = new MemoryCacheEntryOptions()
        {
            SlidingExpiration = expiry,
        };
        _memoryCache.Set(key, Data, slidingExpiration);
        return Data;
    }

    public void DeleteKey(string key)
    {
        _memoryCache.Remove(key);
    }
}
