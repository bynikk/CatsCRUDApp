namespace BLL.Interfaces
{
    public interface IFinder<T> where T : class
    {
        Task<T>? GetById(int id);

    }
}
