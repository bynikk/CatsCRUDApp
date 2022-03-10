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
            catRepository.Create(cat);
            return unitOfWork.Save();
        }

        public Task<List<Cat>> Get()
        {
            return catRepository.GetAll();
        }

        public Task Update(Cat cat)
        {
            catRepository.Update(cat);
            return unitOfWork.Save();
        }

        public Task Delete(Cat cat)
        {
            catRepository.Delete(cat);
            return unitOfWork.Save();
        }

        public Task<Cat> GetCatById(Cat cat)
        {
            return catFinder.GetById(cat);
        }
    }
}
