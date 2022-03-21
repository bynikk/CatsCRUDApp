using BLL.Entities;
using BLL.Interfaces;
using ServiceStack;
using ServiceStack.Redis;

namespace DAL.Finders
{
    public class CatFinderCache : CatFinder, IFinder<Cat>
    {
        ICacheService<Cat> cacheService;

        public CatFinderCache(IPetsContext context, ICacheService<Cat> cacheService) : base(context)
        {
            this.cacheService = cacheService;
        }

        public override Task<Cat>? GetById(int catId)
        {
            Cat? data;
            data = cacheService.Get(catId);

            if (data != null)
                return data.AsTaskResult();
            
            var cat = base.GetById(catId);
            if (cat == null) throw new ArgumentNullException(nameof(cat));

            cacheService.Add(catId, cat.GetAwaiter().GetResult());
            return cat;
          
        }
    }
}
