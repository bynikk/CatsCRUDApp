namespace BLL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        void Create(T item);
        void Update(T item);
        void Delete(T item);
    }
}