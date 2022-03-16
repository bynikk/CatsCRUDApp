using BLL.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IPetsContext
    {
        IMongoCollection<Cat> Cats { get; }
        IMongoCollection<Dog> Dogs { get; }
    }
}
