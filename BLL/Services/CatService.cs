using AutoMapper;
using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class CatService : ICatService
    {
        IUnitOfWork db { get; set; }

        public CatService(IUnitOfWork uow)
        {
            db = uow;
        }

        public void Dispose()
        {
            db.Dispose();
        }
        public void CreateCat(CatDTO catDto)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Cat, CatDTO>()).CreateMapper();
            Cat cat = mapper.Map<CatDTO, Cat>(catDto);
            db.Cats.Create(cat);
            db.Save();
        }

        public CatDTO GetCat(int? id)
        {
            if (id == null)
                throw new ValidationException("Cat ID not set", "");
            var cat = db.Cats.Get(id.Value);
            if (cat == null)
                throw new ValidationException("Cat not set", "");

            return new CatDTO { Id = cat.Id, Name = cat.Name, Price = cat.Price };
        }

        public IEnumerable<CatDTO> GetCats()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Cat, CatDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Cat>, List<CatDTO>>(db.Cats.GetAll());
        }

        public void UpdateCat(CatDTO catDto)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Cat, CatDTO>()).CreateMapper();
            Cat cat = mapper.Map<CatDTO, Cat>(catDto);
            db.Cats.Update(cat);
            db.Save();
        }

        public void DeleteCat(int id)
        {
            if (id == null)
                throw new ValidationException("Cat ID not set", "");
            db.Cats.Delete(id);
            db.Save();
        }
    }
}
