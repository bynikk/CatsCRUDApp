using BLL.Entities;
using BLL.Interfaces;
using DAL.EF;

namespace DAL.Finders
{
    public class CatFinder : IFinder<Cat>
    {
        private CatDbContext db;

        public CatFinder(CatDbContext context)
        {
            db = context;
        }

        public async Task<Cat?> GetById(int id)
        {
            return db.Cats.FirstOrDefault(x => x.Id == id);
        }
    }
}
