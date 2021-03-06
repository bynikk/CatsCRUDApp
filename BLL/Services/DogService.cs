using BLL.Entities;
using BLL.Interfaces;

namespace BLL.Services
{
    public class DogService : IDogService
    {
        IRepository<Dog> dogRepository { get; set; }
        IFinder<Dog> dogFinder { get; set; }

        public DogService(IRepository<Dog> rep, IFinder<Dog> finder)
        {
            dogRepository = rep;
            dogFinder = finder;
        }

        public Task Create(Dog dog)
        {
            return dogRepository.Create(dog);
        }

        public Task<List<Dog>> Get()
        {
            return dogRepository.GetAll();
        }

        public Task Update(Dog dog)
        {
            return dogRepository.Update(dog);
        }

        public Task Delete(int id)
        {
            return dogRepository.Delete(id);
        }

        public Task<Dog> GetCatById(int DogId)
        {
            return dogFinder.GetById(DogId);
        }
    }
}
