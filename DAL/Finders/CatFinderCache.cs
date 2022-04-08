using BLL.Entities;
using BLL.Interfaces;
using ServiceStack;

namespace DAL.Finders
{
    public class CatFinderCache : CatFinder, IFinder<Cat>
    {
        ICache<Cat> cacheService;

        public CatFinderCache(
            IPetsContext context,
            ICache<Cat> cacheService) : base(context)
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

            var resultCat = cat.GetAwaiter().GetResult();
            cacheService.Set(resultCat);
            return cat;
          
        }
    }
}
