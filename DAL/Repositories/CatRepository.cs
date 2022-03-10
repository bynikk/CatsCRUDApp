using BLL.Entities;
using BLL.Interfaces;
using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CatRepository : IRepository<Cat>
    {
        private DbSet<Cat> dbSet;
        CatDbContext context;

        public CatRepository(CatDbContext context, DbSet<Cat> dbSet)
        {
            this.dbSet = dbSet;
            this.context = context;
        }

        public void Create(Cat item)
        {
            dbSet.Add(item);
        }

        public void Delete(Cat cat)
        {
            dbSet.Remove(cat);
        }

        public void Update(Cat item)
        {
            if (context.Entry<Cat>(item).State == EntityState.Detached) {
                dbSet.Attach(item);
            }
            context.Entry<Cat>(item).State = EntityState.Modified;

        }

        public Task<List<Cat>> GetAll()
        {
            return dbSet.ToListAsync();
        }

    }
}