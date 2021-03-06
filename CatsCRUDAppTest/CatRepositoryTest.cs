using BLL.Entities;
using BLL.Interfaces;
using DAL.EF;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatsCRUDAppTest
{
    public class CatRepositoryTest
    {
        private IRepository<Cat>? catRepository;
        private FakeDbContext? context;

        private Cat defaultCat = new Cat() {Id = 0, Name = "name" };


        [SetUp]
        public void SetUp()
        {
            var dbBuilder = new DbContextOptionsBuilder<CatDbContext>();
            dbBuilder.UseInMemoryDatabase(databaseName: "FakeDbContext");
            context = new (dbBuilder.Options);
            catRepository = new CatRepository(context, context.Cats);
        }

        [Test]
        public void CreateTrueTest()
        {
            Cat cat = defaultCat;
            catRepository?.Create(cat);
            context?.SaveChanges();

            var existingCat = context?.Find<Cat>(cat.Id);

            Assert.IsNotNull(existingCat);
        }

        [Test]
        public async Task GetAllTrueTest()
        {
            IEnumerable<Cat> cats = await catRepository.GetAll();

            Assert.AreEqual(context?.Cats, cats);
        }

        [Test]
        public void UpdateTrueTest()
        {
            var cat = new Cat() { Id = 14, Name = "cat14"};
            var updateCat = new Cat() { Id = 14, Name = "cat1" };

            catRepository?.Create(cat);
            context.SaveChanges();

            catRepository?.Update(updateCat);
            context.SaveChanges();
             
            var existingCat = context?.Find<Cat>(updateCat.Id);

            Assert.AreEqual(updateCat.Id, existingCat?.Id);
            Assert.AreEqual(updateCat.Name, existingCat?.Name);
        }

        [Test]
        public void DeleteTrueTest()
        {

            Cat deleteCat = new Cat() { Id = 1, Name = "cat1" };
            catRepository?.Delete(deleteCat);
            context?.SaveChanges();

            var existingCat = context?.Find<Cat>(deleteCat.Id);

            Assert.IsNull(existingCat);
        }
    }
}