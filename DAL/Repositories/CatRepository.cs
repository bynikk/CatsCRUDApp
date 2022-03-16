
using BLL.Entities;
using BLL.Interfaces;
using MongoDB.Driver;

namespace DAL.Repositories
{
    public class CatRepository : IRepository<Cat>
    {
        IPetsContext context;

        public CatRepository(IPetsContext context)
        {
            this.context = context;
        }

        public Task Create(Cat item)
        {
            return context.Cats.InsertOneAsync(item);
        }

        public Task Delete(Cat item)
        {
            return context.Cats.DeleteOneAsync(c => c.Id == item.Id);
        }

        public Task Update(Cat item)
        {
            var filter = Builders<Cat>.Filter.Eq("Id", item.Id);
            var product = context.Cats.Find(filter).FirstOrDefaultAsync();
            
            var update = Builders<Cat>.Update
                                          .Set(x => x.Name, item.Name);

            return context.Cats.UpdateOneAsync(filter, update);
        }

        public Task<List<Cat>> GetAll()
        {
            return context.Cats.Find(_ => true).ToListAsync();
        }

    }
}