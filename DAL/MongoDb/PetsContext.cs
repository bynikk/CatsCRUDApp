using BLL.Entities;
using BLL.Interfaces;
using DAL.Config;
using MongoDB.Driver;

namespace DAL.MongoDb
{
    public class PetsContext : IPetsContext
    {
        private readonly IMongoDatabase database;
        MongoConfig mongoConfig;

        public PetsContext(MongoConfig config)
        {
            mongoConfig = config;

            var client = new MongoClient(mongoConfig.ConnectionString);
            database = client.GetDatabase(mongoConfig.DatabaseName);
        }
        public IMongoCollection<Cat> Cats => database.GetCollection<Cat>(mongoConfig.CatsTableName);
        public IMongoCollection<Dog> Dogs => database.GetCollection<Dog>(mongoConfig.DogsTableName);

    }
}
