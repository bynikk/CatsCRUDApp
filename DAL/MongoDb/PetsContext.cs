using BLL.Entities;
using BLL.Interfaces;
using DAL.Config;
using MongoDB.Driver;

namespace DAL.MongoDb
{
    public class PetsContext : IPetsContext
    {
        private readonly IMongoDatabase database;   

        public PetsContext(Ipconfig config)
        {
            var buff = new MongoDBConfig();
            buff.Port = config.MongoPort;
            buff.Host = config.MongoIp;
            buff.Database = "carscrudapp";

            var client = new MongoClient(buff.ConnectionString);
            database = client.GetDatabase(buff.Database);
        }
        public IMongoCollection<Cat> Cats => database.GetCollection<Cat>("Cats");
        public IMongoCollection<Dog> Dogs => database.GetCollection<Dog>("Dogs");

    }
}
