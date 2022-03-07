using BLL.Entities;
using BLL.Interfaces;
using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CatRepository : IRepository<Cat>
    {
        private CatDbContext db;

        public CatRepository(CatDbContext context)
        {
            db = context;
        }
        public async void Create(Cat item)
        {
            await db.Cats.AddAsync(item);
        }

        public async void Delete(int id)
        {
            var cat = await db.Cats.FirstOrDefaultAsync(x => x.Id == id);
            if (cat != null)
            {
                db.Cats.Remove(cat);
            }
        }
        public async Task<IEnumerable<Cat>> GetAll()
        {
            return db.Cats;
        }

        public async void Update(Cat item)
        {
            var cat = db.Cats.FirstOrDefault(p => p.Id == item.Id);

            cat.Name = item.Name;
        }
    }
}