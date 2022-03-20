using BLL.Entities;
using MongoDB.Driver;

namespace BLL.Interfaces
{
    public interface IPetsContext
    {
        IMongoCollection<Cat> Cats { get; }
        IMongoCollection<Dog> Dogs { get; }
    }
}
