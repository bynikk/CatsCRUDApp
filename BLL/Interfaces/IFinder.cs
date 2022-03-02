
namespace BLL.Interfaces
{
    public interface IFinder<T> where T : class
    {
        Task<IEnumerable<T>> Find(Func<T, Boolean> predicate);
        Task<T> GetById(int id);

    }
}
