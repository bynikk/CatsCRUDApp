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
        private readonly RedisEndpoint redisConfiguration;
        private readonly TimeSpan expirationTime;
        public CatFinder(IPetsContext context)
        {
            this.context = context;
            redisConfiguration = new RedisEndpoint() { Host = "localhost", Port = 6379 };
            expirationTime = TimeSpan.FromSeconds(300);
        }

        public async virtual Task<Cat>? GetById(Cat cat)
        {
            var filter = Builders<Cat>.Filter.Eq("Id", cat.Id);
            var existing = await context.Cats.Find(filter).FirstOrDefaultAsync();

            if (existing == null) return existing;

            string cacheKey = string.Format(existing.Id.ToString(), existing.Name);
            using (IRedisClient client = new RedisClient(redisConfiguration))
            {
                client.Set(cacheKey, existing, expirationTime);
            }

            return existing;
        }
    }
}
