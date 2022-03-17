using BLL.Entities;
using BLL.Interfaces;
using MongoDB.Driver;
using ServiceStack.Redis;

namespace DAL.Repositories
{
    public class CatRepositoryCache : CatRepository
    {
        IPetsContext context;
        private readonly RedisEndpoint redisEndpoint;
        IRedisConfiguration redisConfiguration;


        public CatRepositoryCache(IPetsContext context, IRedisConfiguration configuration) : base(context)
        {
            this.context = context;
            redisEndpoint = new RedisEndpoint() { Host = configuration.Host, Port = configuration.Port };
            redisConfiguration = configuration;

        }

        public override Task Delete(Cat item)
        {
            using (IRedisClient client = new RedisClient(redisEndpoint))
            {
                client.Delete<Cat>(item);
            }

            return base.Delete(item);
        }

        public override Task Update(Cat item)
        {
            var filter = Builders<Cat>.Filter.Eq("Id", item.Id);
            var cat = context.Cats.Find(filter).FirstOrDefaultAsync().GetAwaiter().GetResult();

            if (cat == null) throw new ArgumentNullException();

            using IRedisClient client = new RedisClient(redisEndpoint);
            client.Delete<Cat>(cat);
            var cacheKey = item.Id.ToString(); 
            client.Set(cacheKey, item, redisConfiguration.expirationTime);

            return base.Update(item);
        }

    }
}