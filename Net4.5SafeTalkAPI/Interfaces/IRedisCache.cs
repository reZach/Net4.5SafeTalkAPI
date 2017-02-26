using SafeTalkCore;
using StackExchange.Redis;

namespace Net4._5SafeTalkAPI.Interfaces
{
    public interface IRedisCache
    {
        // https://docs.microsoft.com/en-us/azure/redis-cache/cache-web-app-howto
        ConnectionMultiplexer Connection { get; }
        IDatabase Cache { get; }

        bool DoesCacheExist();
        RedisCache GetCache();
        void SetCache(RedisCache cache);        
    }
}
