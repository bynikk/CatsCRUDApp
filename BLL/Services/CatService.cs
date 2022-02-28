using AutoMapper;
using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    public class CatService : ICatService
    {
        IRepository<Cat> rep { get; set; }

        public CatService(IRepository<Cat> rep)
        {
            this.rep = rep;
        }

        public void CreateCat(CatDTO catDto)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CatDTO, Cat>()).CreateMapper();
            Cat cat = mapper.Map<CatDTO, Cat>(catDto);

            rep.Create(cat);
        }

        public CatDTO GetCat(int id)
        {
            var cat = rep.Get(id);

            return new CatDTO { Id = cat.Id, Name = cat.Name };
        }

        public IEnumerable<CatDTO> GetCats()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Cat, CatDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Cat>, IEnumerable<CatDTO>>(rep.GetAll());
        }

        public void UpdateCat(CatDTO catDto)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CatDTO, Cat>()).CreateMapper();
            Cat cat = mapper.Map<CatDTO, Cat>(catDto);

            rep.Update(cat);
        }

        public void DeleteCat(int id)
        {
            rep.Delete(id);
        }
    }
}
