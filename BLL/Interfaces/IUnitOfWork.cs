using BLL.Entities;

namespace BLL.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> Save();
    }
}
