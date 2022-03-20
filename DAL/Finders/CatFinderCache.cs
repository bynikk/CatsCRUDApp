using BLL.Entities;
using BLL.Interfaces;
using ServiceStack;
using ServiceStack.Redis;

namespace DAL.Finders
{
    public class CatFinderCache : CatFinder, IFinder<Cat>
    {
        private readonly RedisEndpoint redisEndpoint;
        IRedisConfiguration redisConfiguration;

        public CatFinderCache(IPetsContext context, IRedisConfiguration configuration) : base(context, configuration)
        {
            redisConfiguration = configuration;
            redisEndpoint = new RedisEndpoint() { Host = redisConfiguration.Host, Port = redisConfiguration.Port };
        }

        public override Task<Cat>? GetById(int catId)
        {
            string cacheKey = $"{catId}";
            Cat? data;
            using (IRedisClient client = new RedisClient(redisEndpoint))
            {
                data = client.Get<Cat>(cacheKey);
            }

            if (data != null)
                return data.AsTaskResult();
            
            return base.GetById(catId);
          
        }
    }
}
