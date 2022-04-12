using BLL.Entities;
using BLL.Interfaces;
using MongoDB.Driver;

namespace DAL.Repositories
{
    public class CatRepositoryCache : CatRepository
    {
        IPetsContext context;
        ICache<Cat> cache;

        public CatRepositoryCache(
            IPetsContext context,
            ICache<Cat> cacheService) : base(context)
        {
            this.context = context;
            this.cache = cacheService;
        }

        public override Task Delete(int id)
        {
            cache.Delete(id);

            return base.Delete(id);
        }

        public override Task Update(Cat item)
        {
            int cacheKey = item.Id;
            Cat? cat = cache.Get(cacheKey);

            if (cat == null)
            {
                var filter = Builders<Cat>.Filter.Eq("Id", item.Id);
                cat = context.Cats.Find(filter).FirstOrDefaultAsync().GetAwaiter().GetResult();
            }

            if (cat == null) throw new ArgumentNullException();

            cache.Delete(cacheKey);
            cache.Set(item);

            return base.Update(item);
        }

    }
}