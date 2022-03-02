using BLL.Models;

namespace BLL.Interfaces
{
    public interface ICatService
    {
        Task<CatViewModel> GetCat(int id);
        Task CreateCat(CatViewModel catDto);
        Task UpdateCat(CatViewModel catDto);
        Task DeleteCat(int id);
        Task<IEnumerable<CatViewModel>> GetCats();
    }
}
