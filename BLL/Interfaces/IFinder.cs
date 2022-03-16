
using BLL.Entities;

namespace BLL.Interfaces
{
    public interface IFinder<T> where T : class
    {
        Task<T> GetById(T item);

    }
}
