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
        IRepository<Cat> catRepository { get; set; }

        public CatService(IRepository<Cat> rep)
        {
            this.catRepository = rep;
        }

        public async Task CreateCat(CatDTO catDto)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CatDTO, Cat>()).CreateMapper();
            Cat cat = mapper.Map<CatDTO, Cat>(catDto);

            await catRepository.Create(cat);
        }

        public async Task<CatDTO> GetCat(int id)
        {
            Cat cat = await catRepository.Get(id);

            if (cat == null)
                throw new ValidationException("No cat in context with this id", "");

            return new CatDTO { Id = cat.Id, Name = cat.Name };
        }

        public async Task<IEnumerable<CatDTO>> GetCats()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Cat, CatDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Cat>, IEnumerable<CatDTO>>(await catRepository.GetAll());
        }

        public async Task UpdateCat(CatDTO catDto)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CatDTO, Cat>()).CreateMapper();
            Cat cat = mapper.Map<CatDTO, Cat>(catDto);

            await catRepository.Update(cat);
        }

        public async Task DeleteCat(int id)
        {
            await catRepository.Delete(id);
        }
    }
}
