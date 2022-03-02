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

        public async Task CreateCat(Cat cat)
        {
            await catRepository.Create(cat);
        }

        public async Task<Cat> GetCat(int id)
        {
            Cat cat = await catRepository.Get(id);

            return new Cat { Id = cat.Id, Name = cat.Name };
        }

        public async Task<IEnumerable<Cat>> GetCats()
        {
            
            return await catRepository.GetAll();
        }

        public async Task UpdateCat(Cat cat)
        {
            await catRepository.Update(cat);
        }

        public async Task DeleteCat(int id)
        {
            await catRepository.Delete(id);
        }
    }
}
