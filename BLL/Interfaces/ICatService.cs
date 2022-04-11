using BLL.Entities;

namespace BLL.Interfaces
{
    public interface ICatService
    {
        Task Create(Cat cat);
        Task Update(Cat cat);
        Task Delete(int id);
        Task<List<Cat>> Get();
        Task<Cat> GetCatById(int catId);
    }
}
