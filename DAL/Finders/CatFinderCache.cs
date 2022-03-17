using BLL.Entities;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ServiceStack;
using ServiceStack.Redis;

namespace DAL.Finders
{
    public class CatFinderCache : CatFinder, IFinder<Cat>
    {
        private readonly RedisEndpoint redisConfiguration;

        public CatFinderCache(IPetsContext context) : base(context)
        {
            redisConfiguration = new RedisEndpoint() { Host = "localhost", Port = 6379 };
        }

        public override Task<Cat>? GetById(Cat item)
        {
            string cacheKey = $"{item.Id}{item.Name}";
            Cat? data;
            using (IRedisClient client = new RedisClient(redisConfiguration))
            {
                data = client.Get<Cat>(cacheKey);
            }

            if (data != null)
                return data.AsTaskResult();
            
            return base.GetById(item);
          
        }
    }
}
