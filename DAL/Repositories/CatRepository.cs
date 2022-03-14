using BLL.Entities;
using BLL.Interfaces;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DAL.Repositories
{
    public class CatRepository : IRepository<Cat>
    {
        private DbSet<Cat> dbSet;
        CatDbContext context;
        IPetsContext petsContext;

        public CatRepository(CatDbContext context, DbSet<Cat> dbSet, IPetsContext petsContext)
        {
            this.dbSet = dbSet;
            this.context = context;
            this.petsContext = petsContext;
        }

        public void Create(Cat item)
        {
            petsContext.Cats.InsertOne(item);
        }

        public void Delete(Cat cat)
        {
            petsContext.Cats.DeleteOne(c => c.Id == cat.Id);
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
            return petsContext.Cats.Find(_ => true).ToListAsync();
        }

    }
}