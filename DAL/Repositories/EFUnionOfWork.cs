using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private ShelterDbContext db;
        private CatRepository catRepository;
        private OrderRepository orderRepository;

        public EFUnitOfWork(DbContextOptions<ShelterDbContext> options)
        {
            db = new ShelterDbContext(options);
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

        public IRepository<Order> Orders
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new OrderRepository(db);
                return orderRepository;
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
