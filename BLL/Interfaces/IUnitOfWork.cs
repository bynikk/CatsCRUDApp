using BLL.Entities;

namespace BLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Cat> Cats { get; }
        void Save();
    }
}
