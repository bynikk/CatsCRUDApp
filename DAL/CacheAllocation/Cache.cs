using BLL.Entities;
using BLL.Interfaces;
using StackExchange.Redis;

namespace DAL.CacheAllocation
{
    public class Cache : ICache<Cat>
    {
        private Dictionary<int, WeakReference> cacheDictionary;

        ConnectionMultiplexer connectionMultiplexer;
        IDatabase redisDb;
        string streamName = "telemetry";

        public Cache()
        {
            cacheDictionary = new ();
            connectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379");
            redisDb = connectionMultiplexer.GetDatabase();
        }
        public void Set(int key, Cat item)
        {
            cacheDictionary.Add(key, new WeakReference(item));
            redisDb.StreamAdd(streamName, new NameValueEntry[] {
                new NameValueEntry("id", item.Id),
                new NameValueEntry("name", item.Name),
                new NameValueEntry("date", item.CreatedDate.ToString()),
            });
        }

        public Cat? Get(int key)
        { 
            if (cacheDictionary.ContainsKey(key) && cacheDictionary[key].IsAlive)
            {
                return cacheDictionary[key].Target as Cat;
            }
            return null;
        }

        public void Delete(int key)
        {
            var result = redisDb.StreamRange(streamName, "-", "+").FirstOrDefault(c => c.Values[0].Value == key);
            if (!result.IsNull) redisDb.StreamDelete(streamName, new RedisValue[] { result.Id });

            cacheDictionary.Remove(key);
        }
    }
}
