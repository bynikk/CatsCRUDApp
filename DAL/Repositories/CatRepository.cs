using BLL.Entities;
using BLL.Interfaces;
using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CatRepository : IRepository<Cat>
    {
        private DbSet<Cat> dbSet;

        public CatRepository(DbSet<Cat> dbSet)
        {
            this.dbSet = dbSet;
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
            dbSet.Attach(item).State = EntityState.Modified;

        }

        public Task<List<Cat>> GetAll()
        {
            return dbSet.ToListAsync();
        }

    }
}