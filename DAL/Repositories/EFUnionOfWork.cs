using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private CatDbContext db;
        private CatRepository catRepository;

        public EFUnitOfWork(DbContextOptions<CatDbContext> options)
        {
            db = new CatDbContext(options);
        }
        public IRepository<Cat> Cats
        {
            get
            {
                if (catRepository == null)
                    catRepository = new CatRepository(db);
                return catRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
