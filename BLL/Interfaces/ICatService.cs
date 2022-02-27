using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICatService
    {
        void CreateCat(CatDTO catDto);
        CatDTO GetCat(int? id);
        void UpdateCat(CatDTO catDto);
        void DeleteCat(int id);
        IEnumerable<CatDTO> GetCats();
        void Dispose();
    }
}
