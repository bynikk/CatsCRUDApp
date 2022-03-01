using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CatRepository : IRepository<Cat>
    {
        private CatDbContext db;

        public CatRepository(CatDbContext context)
        {
            this.db = context;
        }
        public async Task CreateAsync(Cat item)
        {
            await db.Cats.AddAsync(new Cat { Id = item.Id, Name = item.Name, CreatedDate = DateTime.Now });
            await Task.Run(() => db.SaveChangesAsync());
        }

        public async Task DeleteAsync(int id)
        {
            var cat = await db.Cats.FirstOrDefaultAsync(x => x.Id == id);
            if (cat != null)
            {
                db.Cats.Remove(cat);
                await Task.Run(() => db.SaveChangesAsync());
            }
        }

        public IEnumerable<Cat> FindAsync(Func<Cat, bool> predicate)
        {
            return Task.Run(() => db.Cats.Where(predicate)).GetAwaiter().GetResult();
        }

        public async Task<Cat?> GetAsync(int id)
        {
            return await Task.Run(() => db.Cats.FirstOrDefaultAsync(x => x.Id == id));
        }

        public IEnumerable<Cat> GetAllAsync()
        {
            return Task.Run(() => db.Cats).GetAwaiter().GetResult();
        }

        public async Task UpdateAsync(Cat item)
        {
            var cat = await db.Cats.FirstOrDefaultAsync(p => p.Id == item.Id);

            cat.Id = item.Id;
            cat.Name = item.Name;

            await Task.Run(() => db.SaveChangesAsync());
        }
    }
}