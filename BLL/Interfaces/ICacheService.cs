namespace BLL.Interfaces
{
    public interface ICacheService<T> where T : class
    {
        public void Add(int key, T value);

        public T? Get(int key);

        public void Delete(int key);
    }
}
