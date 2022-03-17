using BLL.Entities;
using BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DAL.Finders
{
    public class DogFinder : IFinder<Dog>
    {
        private IPetsContext context;

        public DogFinder(IPetsContext context)
        {
            this.context = context;
        }

        public Task<Dog> GetById(int dogId)
        {
            var filter = Builders<Dog>.Filter.Eq("Id", dogId);
            return context.Dogs.Find(filter).FirstOrDefaultAsync();
        }
    }
}
