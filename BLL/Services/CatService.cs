using BLL.Entities;
using BLL.Interfaces;

namespace BLL.Services
{
    public class CatService : ICatService
    {
        IRepository<Cat> catRepository { get; set; }
        IFinder<Cat> catFinder { get; set; }

        public CatService(
            IRepository<Cat> catRepository,
            IFinder<Cat> catFinder)
        {
            this.catRepository = catRepository;
            this.catFinder = catFinder;
        }

        public Task Create(Cat cat)
        {
            return catRepository.Create(cat);
        }

        public Task<List<Cat>> Get()
        {
            return catRepository.GetAll();
        }

        public Task Update(Cat cat)
        {
            return catRepository.Update(cat);
        }

        public Task Delete(int id)
        {
            return catRepository.Delete(id);
        }

        public Task<Cat> GetCatById(int catId)
        {
            return catFinder.GetById(catId);
        }
    }
}
