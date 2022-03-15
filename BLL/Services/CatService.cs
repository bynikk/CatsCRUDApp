using BLL.Entities;
using BLL.Interfaces;

namespace BLL.Services
{
    public class CatService : ICatService
    {
        IRepository<Cat> catRepository { get; set; }
        IFinder<Cat> catFinder { get; set; }
        IUnitOfWork unitOfWork { get; set; }

        public CatService(IRepository<Cat> rep, IFinder<Cat> finder, IUnitOfWork uow)
        {
            catRepository = rep;
            catFinder = finder;
            unitOfWork = uow;
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

        public Task Delete(Cat cat)
        {
            return catRepository.Delete(cat);
        }

        public Task<Cat> GetCatById(Cat cat)
        {
            return catFinder.GetById(cat);
        }
    }
}
