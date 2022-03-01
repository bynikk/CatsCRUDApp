using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ICatService
    {
        Task<CatDTO> GetCat(int id);
        Task CreateCat(CatDTO catDto);
        Task UpdateCat(CatDTO catDto);
        Task DeleteCat(int id);
        Task<IEnumerable<CatDTO>> GetCats();
    }
}
