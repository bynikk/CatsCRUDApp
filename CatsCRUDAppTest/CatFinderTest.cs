using BLL.Entities;
using BLL.Interfaces;
using DAL.EF;
using DAL.Finders;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsCRUDAppTest
{
    class CatFinderTest
    {
        private IFinder<Cat>? catFinder;
        private IRepository<Cat>? catRepository;
        private FakeDbContext? context;

        private Cat defaultCat = new Cat() { Id = 0, Name = "name" };


        [SetUp]
        public void SetUp()
        {
            var dbBuilder = new DbContextOptionsBuilder<CatDbContext>();
            dbBuilder.UseInMemoryDatabase(databaseName: "FakeDbContext");
            context = new(dbBuilder.Options);

            catFinder = new CatFinder(context);
            catRepository = new CatRepository(context);
        }

        [Test]
        public void GetByIdTrueTest()
        {
            Cat cat = defaultCat;
            catRepository?.Create(cat);
            context?.SaveChanges();
            int id = cat.Id;

            var findedCat = catFinder?.GetById(cat.Id);
            var expectedCat = context?.Find<Cat>(cat.Id);

            Assert.AreEqual(expectedCat, findedCat?.Result);
        }
    }
}
