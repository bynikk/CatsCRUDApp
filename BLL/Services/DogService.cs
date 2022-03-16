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

        public Task Delete(Dog dog)
        {
            return dogRepository.Delete(dog);
        }

        public Task<Dog> GetCatById(Dog dog)
        {
            return dogFinder.GetById(dog);
        }
    }
}
