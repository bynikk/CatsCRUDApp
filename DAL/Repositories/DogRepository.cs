
using BLL.Entities;
using BLL.Interfaces;
using MongoDB.Driver;

namespace DAL.Repositories
{
    public class DogRepository : IRepository<Dog>
    {
        IPetsContext context;

        public DogRepository(IPetsContext context)
        {
            this.context = context;
        }

        public Task Create(Dog item)
        {
            return context.Dogs.InsertOneAsync(item);
        }

        public Task Delete(Dog item)
        {
            return context.Dogs.DeleteOneAsync(c => c.Id == item.Id);
        }

        public Task Update(Dog item)
        {
            var filter = Builders<Dog>.Filter.Eq("Id", item.Id);
            var product = context.Dogs.Find(filter).FirstOrDefaultAsync();

            var update = Builders<Dog>.Update
                                          .Set(x => x.Name, item.Name)
                                          .Set(x => x.Breed, item.Breed);

            return context.Dogs.UpdateOneAsync(filter, update);
        }

        public Task<List<Dog>> GetAll()
        {
            return context.Dogs.Find(_ => true).ToListAsync();
        }

    }
}