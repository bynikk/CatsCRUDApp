using BLL.Entities;

namespace BLL.Interfaces
{
    public interface IDogService
    {
        Task Create(Dog dog);
        Task Update(Dog dog);
        Task Delete(Dog dog);
        Task<List<Dog>> Get();
        Task<Dog> GetCatById(int dogId);
    }
}
