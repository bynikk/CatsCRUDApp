using BLL.Entities;
using BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using ServiceStack.Redis;

namespace DAL.Finders
{
    public class CatFinder
    {
        private IPetsContext context;
        private readonly RedisEndpoint redisEndpoint;
        IRedisConfiguration redisConfigurationl;

        public CatFinder(IPetsContext context, IRedisConfiguration configuration)
        {
            this.context = context;
            redisConfigurationl = configuration;
            redisEndpoint = new RedisEndpoint() { Host = redisConfigurationl.Host, Port = redisConfigurationl.Port };
        }

        public async virtual Task<Cat>? GetById(int catId)
        {
            var filter = Builders<Cat>.Filter.Eq("Id", catId);
            var existing = await context.Cats.Find(filter).FirstOrDefaultAsync();

            if (existing == null) throw new ArgumentNullException();

            string cacheKey = string.Format(existing.Id.ToString(), existing.Name);
            using (IRedisClient client = new RedisClient(redisEndpoint))
            {
                client.Set(cacheKey, existing, redisConfigurationl.expirationTime);
            }

            return existing;
        }
    }
}
