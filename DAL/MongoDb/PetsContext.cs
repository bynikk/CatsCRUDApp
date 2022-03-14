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
            var client = new MongoClient("mongodb://admin:p%40ssw0rd@localhost:27017/?authSource=admin");
            database = client.GetDatabase("carscrudapp");
        }
        public IMongoCollection<Cat> Cats => database.GetCollection<Cat>("Cats");
        public IMongoCollection<Dog> Dogs => database.GetCollection<Dog>("Dogs");
    }
}
