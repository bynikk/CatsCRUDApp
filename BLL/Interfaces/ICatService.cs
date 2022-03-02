using BLL.Entities;

namespace BLL.Interfaces
{
    public interface ICatService
    {
        Task CreateCat(Cat catDto);
        Task UpdateCat(Cat catDto);
        Task DeleteCat(int id);
        Task<IEnumerable<Cat>> GetCats();
        Task<IEnumerable<Cat>> FindCats(Func<Cat, Boolean> predicate);
        Task<Cat> GetCatById(int id);
    }
}
