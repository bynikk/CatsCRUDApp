using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Cat> Cats { get; }
        IRepository<Order> Orders { get; }
        void Save();
    }
}
