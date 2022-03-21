using BLL.Entities;
using BLL.Interfaces;
using MongoDB.Driver;
using ServiceStack.Redis;

namespace DAL.Repositories
{
    public class CatRepositoryCache : CatRepository
    {
        IPetsContext context;
        ICacheService<Cat> cacheService;

        public CatRepositoryCache(IPetsContext context, ICacheService<Cat> cacheService) : base(context)
        {
            this.context = context;
            this.cacheService = cacheService;
        }

        public override Task Delete(Cat item)
        {
            cacheService.Delete(item.Id);

            return base.Delete(item);
        }

        public override Task Update(Cat item)
        {
            int cacheKey = item.Id;
            Cat cat = cacheService.Get(cacheKey);

            if (cat == null)
            {
                var filter = Builders<Cat>.Filter.Eq("Id", item.Id);
                cat = context.Cats.Find(filter).FirstOrDefaultAsync().GetAwaiter().GetResult();
            }

            if (cat == null) throw new ArgumentNullException();

            cacheService.Delete(cacheKey);
            cacheService.Add(cacheKey, item);

            return base.Update(item);
        }

    }
}