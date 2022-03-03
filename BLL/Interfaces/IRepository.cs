namespace BLL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}