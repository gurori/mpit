using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace mpit.mpit.Infastructure.Cache;

public sealed class CacheClient(IDistributedCache cache)
{
    private readonly IDistributedCache _cache = cache;

    public async Task<T?> GetByKeyAsync<T>(string key)
        where T : class
    {
        var str = await _cache.GetStringAsync(key);
        if (str is null)
            return null;

        var obj = JsonSerializer.Deserialize<T>(str);
        return obj;
    }

    public async Task SetAsync(string key, object value)
    {
        var str = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, str);
    }
}
