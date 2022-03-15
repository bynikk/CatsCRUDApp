using BLL.Entities;
using BLL.Interfaces;
using MongoDB.Driver;

namespace DAL.MongoDb
{
    public class PetsContext : IPetsContext
    {
        private readonly IMongoDatabase database;   

        public PetsContext()
        {
            var config = new MongoDBConfig();
            config.Port = 27017;
            config.Host = "localhost";
            config.Database = "carscrudapp";

            var client = new MongoClient(config.ConnectionString);
            database = client.GetDatabase(config.Database);
        }
        public IMongoCollection<Cat> Cats => database.GetCollection<Cat>("Cats");
        public IMongoCollection<Dog> Dogs => database.GetCollection<Dog>("Dogs");

    }
}
