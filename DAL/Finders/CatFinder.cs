using BLL.Entities;
using BLL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Finders
{
    public class CatFinder : IFinder<Cat>
    {
        private DbSet<Cat> dbSet;

        public CatFinder(DbSet<Cat> dbSet)
        {
            this.dbSet = dbSet;
        }

        public Task<Cat?> GetById(Cat cat)
        {
            return dbSet.FirstOrDefaultAsync(x => x.Id == cat.Id);
        }
    }
}
