namespace BLL.Interfaces
{
    public interface ICache<T> where T : class
    {
        public void Set(T value);

        public T? Get(int key);

        public void Delete(int key);
    }
}
