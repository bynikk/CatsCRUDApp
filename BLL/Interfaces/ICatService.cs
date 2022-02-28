using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ICatService
    {
        void CreateCat(CatDTO catDto);
        CatDTO GetCat(int id);
        void UpdateCat(CatDTO catDto);
        void DeleteCat(int id);
        IEnumerable<CatDTO> GetCats();
    }
}
