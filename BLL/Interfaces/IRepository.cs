namespace BLL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task Create(T item);
        Task Update(T item);
        Task Delete(int id);
    }
}