using BLL.Entities;
using BLL.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.MongoDb
{
    public class PetsContext : IPetsContext
    {
        private readonly IMongoDatabase database;   

        public PetsContext()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            database = client.GetDatabase("carscrudapp");
            var listCollections = database.ListCollectionsAsync();
        }
        public IMongoCollection<Cat> Cats => database.GetCollection<Cat>("Cats");
        public IMongoCollection<Dog> Dogs => database.GetCollection<Dog>("Dogs");

    }
}
