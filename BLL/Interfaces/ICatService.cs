using DAL.Entities;

namespace BLL.Interfaces
{
    public interface ICatService
    {
        Task<Cat> GetCat(int id);
        Task CreateCat(Cat catDto);
        Task UpdateCat(Cat catDto);
        Task DeleteCat(int id);
        Task<IEnumerable<Cat>> GetCats();
    }
}
