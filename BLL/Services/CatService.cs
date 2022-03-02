using BLL.Entities;
using BLL.Interfaces;

namespace BLL.Services
{
    public class CatService : ICatService
    {
        IRepository<Cat> catRepository { get; set; }
        IFinder<Cat> catFinder { get; set; }

        public CatService(IRepository<Cat> rep, IFinder<Cat> finder)
        {
            catRepository = rep;
            catFinder = finder;
        }

        public async Task CreateCat(Cat cat)
        {
            await catRepository.Create(cat);
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

        public async Task<IEnumerable<Cat>> FindCats(Func<Cat, Boolean> predicate)
        {
            return await catFinder.Find(predicate);
        }
        public async Task<Cat> GetCatById(int id)
        {
            var cat = await catFinder.GetById(id);

            return new Cat { Id = cat.Id, Name = cat.Name };
        }
    }
}
