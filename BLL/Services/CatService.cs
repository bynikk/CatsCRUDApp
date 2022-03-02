using AutoMapper;
using BLL.Infrastructure;
using BLL.Interfaces;
using BLL.Models;
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

        public async Task CreateCat(CatViewModel catDto)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CatViewModel, Cat>()).CreateMapper();
            Cat cat = mapper.Map<CatViewModel, Cat>(catDto);

            await catRepository.Create(cat);
        }

        public async Task<CatViewModel> GetCat(int id)
        {
            Cat cat = await catRepository.Get(id);

            return new CatViewModel { Id = cat.Id, Name = cat.Name };
        }

        public async Task<IEnumerable<CatViewModel>> GetCats()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Cat, CatViewModel>()).CreateMapper();
            return mapper.Map<IEnumerable<Cat>, IEnumerable<CatViewModel>>(await catRepository.GetAll());
        }

        public async Task UpdateCat(CatViewModel catDto)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CatViewModel, Cat>()).CreateMapper();
            Cat cat = mapper.Map<CatViewModel, Cat>(catDto);

            await catRepository.Update(cat);
        }

        public async Task DeleteCat(int id)
        {
            await catRepository.Delete(id);
        }
    }
}
