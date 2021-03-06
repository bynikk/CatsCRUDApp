using BLL.Entities;
using BLL.Interfaces;
using DAL.CacheAllocation;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DAL.Finders
{
    public class CatFinder
    {
        private IPetsContext context;

        public CatFinder(IPetsContext context)
        {
            this.context = context;
        }

        public async virtual Task<Cat>? GetById(int catId)
        {
            var filter = Builders<Cat>.Filter.Eq(FieldNames.Id, catId);
            var existing = await context.Cats.Find(filter).FirstOrDefaultAsync();

            if (existing == null) throw new ArgumentNullException();

            return existing;
        }
    }
}
