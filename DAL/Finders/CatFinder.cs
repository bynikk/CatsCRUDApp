using BLL.Entities;
using BLL.Interfaces;
using DAL.EF;
using System.Data.Entity;

namespace DAL.Finders
{
    public class CatFinder : IFinder<Cat>
    {
        private CatDbContext db;

        public CatFinder(CatDbContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<Cat>> Find(Func<Cat, bool> predicate)
        {
            return db.Cats.Where(predicate);
        }

        public async Task<Cat?> GetById(int id)
        {
            return db.Cats.FirstOrDefault(x => x.Id == id);
        }
    }
}
