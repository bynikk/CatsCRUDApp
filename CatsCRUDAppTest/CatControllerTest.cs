using BLL.Entities;
using BLL.Interfaces;
using BLL.Services;
using CatsCRUDApp.Controllers;
using CatsCRUDApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsCRUDAppTest
{
    public class CatControllerTest
    {
        Mock<IUnitOfWork> mockUnitOfWork;
        Mock<IFinder<Cat>> mockFinder;
        Mock<IRepository<Cat>> mockRepository;

        private CatViewModel defaultCatViewModel = new CatViewModel() { Id = 0, Name = "name", Description = ""};

        [SetUp]
        public void SetUp()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockFinder = new Mock<IFinder<Cat>>();
            mockRepository = new Mock<IRepository<Cat>>();
        }

        [Test]
        public void GetReturnsAllCatsTrueTest()
        {
            mockRepository.Setup(r => r.GetAll()).Returns(GetTestCats());

            CatService fakeCatService = new CatService(mockRepository.Object, mockFinder.Object, mockUnitOfWork.Object);

            var controller = new CatController(fakeCatService);

            var result = controller.Get();


            Assert.AreEqual(result.GetType(), typeof(Task<IActionResult>));

            var model = result.Result as OkObjectResult;

            var cats = model.Value as IEnumerable<CatViewModel>;

            Assert.AreEqual(GetTestCats().Result.Count(), cats.Count());
        }

        [Test]
        public void PostTrueTest()
        {
            mockRepository.Setup(r => r.Create(It.IsAny<Cat>()));

            CatService fakeCatService = new CatService(mockRepository.Object, mockFinder.Object, mockUnitOfWork.Object);
            var controller = new CatController(fakeCatService);

            var result = controller.Post(defaultCatViewModel);

            mockRepository.Verify(r => r.Create(It.IsAny<Cat>()), Times.Once());
            Assert.AreEqual(result.GetType(), typeof(Task<IActionResult>));
        }

        [Test]
        public void DeleteTrueTest()
        {
            mockRepository.Setup(r => r.Delete(It.IsAny<Cat>()));
            mockFinder.Setup(r => r.GetById(It.IsAny<Cat>())).Returns(GetTestCat());

            CatService fakeCatService = new CatService(mockRepository.Object, mockFinder.Object, mockUnitOfWork.Object);
            var controller = new CatController(fakeCatService);

            controller.Delete(defaultCatViewModel);

            mockRepository.Verify(r => r.Delete(It.IsAny<Cat>()), Times.Once);
        }

        [Test]
        public void PutTrueTest()
        {
            mockRepository.Setup(r => r.Update(It.IsAny<Cat>()));

            CatService fakeCatService = new CatService(mockRepository.Object, mockFinder.Object, mockUnitOfWork.Object);
            var controller = new CatController(fakeCatService);

            var result = controller.Put(defaultCatViewModel);

            mockRepository.Verify(r => r.Update(It.IsAny<Cat>()), Times.Once());
            Assert.AreEqual(result.GetType(), typeof(Task<IActionResult>));
        }

        private async Task<List<Cat>> GetTestCats()
        {
            var cats = new List<Cat>
            {
                new Cat { Id=0, Name="Tom"},
                new Cat { Id=1, Name="Alice"},
                new Cat { Id=2, Name="Sam"},
                new Cat { Id=3, Name="Kate"}
            };
            return cats;
        }
        private async Task<Cat> GetTestCat()
        {
            return new Cat() { Id = 11, Name = "Tom" };
        }
    }
}
