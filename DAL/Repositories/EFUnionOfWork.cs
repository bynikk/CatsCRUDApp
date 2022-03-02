using BLL.Entities;
using BLL.Interfaces;
using DAL.EF;
using DAL.Finders;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private CatDbContext db;

        public EFUnitOfWork(CatDbContext context)
        {
            db = context;
        }

        public Task<int> Save()
        {
            return db.SaveChangesAsync();
        }
    }
}
