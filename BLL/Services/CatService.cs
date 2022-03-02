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

        public async Task Create(Cat cat)
        {
            await catRepository.Create(cat);
            await unitOfWork.Save();
        }

        public async Task<IEnumerable<Cat>> Get()
        {
            return await catRepository.GetAll();
        }

        public async Task Update(Cat cat)
        {
            await catRepository.Update(cat);
            await unitOfWork.Save();
        }

        public async Task Delete(int id)
        {
            await catRepository.Delete(id);
            await unitOfWork.Save();
        }

        public async Task<Cat> GetCatById(int id)
        {
            return await catFinder.GetById(id);
        }
    }
}
