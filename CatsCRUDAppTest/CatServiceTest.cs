using BLL.Entities;
using BLL.Interfaces;
using BLL.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatsCRUDAppTest
{
    public class CatServiceTest
    {
        Mock<IUnitOfWork> mockUnitOfWork;
        Mock<IFinder<Cat>> mockFinder;
        Mock<IRepository<Cat>> mockRepository;

        private Cat defaultCat = new Cat() { Id = 0, Name = "name" };

        [SetUp]
        public void SetUp()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockFinder = new Mock<IFinder<Cat>>();
            mockRepository = new Mock<IRepository<Cat>>();
        }

        [Test]
        public void GetTrueTest()
        {
            mockRepository.Setup(r => r.GetAll()).Returns(GetTestCats);

            CatService fakeCatService = new CatService(mockRepository.Object, mockFinder.Object, mockUnitOfWork.Object);

            var cats = fakeCatService.Get();

            Assert.AreEqual(cats.Result.Count(), GetTestCats().Result.Count());
            Assert.IsNotNull(cats);

        }

        [Test]
        public void CreateTrueTest()
        {
            mockRepository.Setup(r => r.Create(It.IsAny<Cat>()));

            CatService fakeCatService = new CatService(mockRepository.Object, mockFinder.Object, mockUnitOfWork.Object);

            fakeCatService.Create(defaultCat);

            mockRepository.Verify(r => r.Create(It.IsAny<Cat>()), Times.Once());

        }

        [Test]
        public void UpdateTrueTest()
        {
            mockRepository.Setup(r => r.Update(It.IsAny<Cat>()));

            CatService fakeCatService = new CatService(mockRepository.Object, mockFinder.Object, mockUnitOfWork.Object);

            fakeCatService.Update(defaultCat);

            mockRepository.Verify(r => r.Update(It.IsAny<Cat>()), Times.Once);

        }

        [Test]
        public void DeleteTrueTest()
        {
            mockRepository.Setup(r => r.Delete(It.IsAny<Cat>()));

            CatService fakeCatService = new CatService(mockRepository.Object, mockFinder.Object, mockUnitOfWork.Object);

            fakeCatService.Delete(defaultCat);

            mockRepository.Verify(r => r.Delete(It.IsAny<Cat>()), Times.Once);

        }

        [Test]
        public void GetCatByIdTest()
        {
            mockFinder.Setup(r => r.GetById(It.IsAny<Cat>())).Returns(GetTestCat);

            CatService fakeCatService = new CatService(mockRepository.Object, mockFinder.Object, mockUnitOfWork.Object);

            var expected = GetTestCat().Result;

            Assert.AreEqual(expected.Name, fakeCatService.GetCatById(expected).Result.Name);
            Assert.AreEqual(expected.Id, fakeCatService.GetCatById(expected).Result.Id);
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
