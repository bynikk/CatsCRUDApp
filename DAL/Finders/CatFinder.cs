using BLL.Entities;
using BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DAL.Finders
{
    public class CatFinder : IFinder<Cat>
    {
        private IPetsContext context;

        public CatFinder(IPetsContext context)
        {
            this.context = context;
        }

        public Task<Cat> GetById(Cat cat)
        {
            var filter = Builders<Cat>.Filter.Eq("Id", cat.Id);
            return context.Cats.Find(filter).FirstOrDefaultAsync();
        }
    }
}
