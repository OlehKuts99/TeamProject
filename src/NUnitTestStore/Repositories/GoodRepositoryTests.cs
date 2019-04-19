using DAL.Models;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using DAL.Classes.UnitOfWork;
using DAL.Classes.UnitOfWork.Classes;
using Microsoft.EntityFrameworkCore;
using Moq;
using DAL.Classes.UnitOfWork.Interfaces;
using System.Collections.Generic;

namespace NUnitTestStore.Repositories
{
    [TestFixture]
    class GoodRepositoryTests
    {
        private Mock<IRepository<Good>> repoMock;
        DbContextOptions<AppDbContext> options;
        [SetUp]
        public void Setup()
        {
            options= new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: "InMemoryDatabase").Options;
            repoMock = new Mock<IRepository<Good>>();
        }
        

        [Test]
        public async Task CreateTest()
        {
            //Arrange
            using (var context = new AppDbContext(options))
            {
                var good = new Good { Id = 1, Name = "Test" };
                var repo = new GoodRepository(context);

                //Act
                await repo.Create(good);
                var expectedResult = 1;
                var actualResult = context.Goods.Local.Count();

                //Assert
                Assert.AreEqual(expectedResult, actualResult);
            }
        }

        [Test]
        public async Task DeleteTest()
        {
            //Arrange
            using (var context = new AppDbContext(options))
            {
                var good = new Good { Id = 1, Name = "Test" };
                var repo = new GoodRepository(context);

                //Act
                var expectedResult = 0;
                context.Goods.Add(good);
                await repo.Delete(good.Id);
                var actualResult = context.Goods.Local.Count();

                //Assert
                Assert.AreEqual(expectedResult, actualResult);
            }
        }

        [Test]
        public async Task GetTest()
        {
            //Arrange
            using (var context = new AppDbContext(options))
            {
                var good = new Good { Id = 1, Name = "Test" };
                var repo = new GoodRepository(context);

                //Act
                var expectedResult = good;
                context.Goods.Add(good);
                var result = await repo.Get(good.Id);

                //Assert
                Assert.AreEqual(expectedResult, result);
            }
        }

        [Test]
        public void GetAllTest()
        {
            //Arrange
            using (var context = new AppDbContext(options))
            {
                var goods = new List<Good> { new Good { Id = 1, }, new Good { Id = 2 }, new Good { Id = 3 } };
                var repo = new GoodRepository(context);

                //Act
                foreach (var good in goods)
                {
                    context.Goods.Add(good);
                }
                var result = repo.GetAll();

                //Assert
                Assert.AreEqual(context.Goods, result);
            }
        }

        [Test]
        public void UpdateTest()
        {
            //Arrange
            using (var context = new AppDbContext(options))
            {
                var good = new Good { Id = 1, Name = "Test" };
                var repo = new GoodRepository(context);

                //Act
                repo.Update(good);
                var expectedResult = EntityState.Modified;
                var actualResult = context.Entry(good).State;

                //Assert
                Assert.AreEqual(expectedResult, actualResult);
            }
        }

        [Test]
        public async Task AddGoodToStorageTest()
        {
            //Arrange
            using (var context = new AppDbContext(options))
            {
                var storages = new List<Storage> { new Storage { Id = 1, }, new Storage { Id = 2 },
                    new Storage { Id = 3 } };
                var good = new Good { Id = 1, Name = "Test" };
                var repo = new GoodRepository(context);

                //Act
                await repo.Create(good);
                await repo.AddGoodToStorage(good.Id, storages);
                var actualResult = context.GoodStorage.Local.Count();
                var expectedResult = 3;

                //Assert
                Assert.AreEqual(expectedResult, actualResult);
            }
        }
        

        [Test]
        public async Task AddReview()
        {
            //Arrange
            using (var context = new AppDbContext(options))
            {
                var review = new GoodReview {  Id = 1, GoodId = 1 };
                var good = new Good { Id = 1, Name = "Test" };
                var repo = new GoodRepository(context);

                //Act
                await repo.Create(good);
                context.Reviews.Add(review);
                await repo.AddReview(review, good);
                var actualResult = context.Reviews.Find(review.Id);
                var expectedResult = review;

                //Assert
                Assert.AreEqual(expectedResult, actualResult);
                Assert.AreEqual(good.Reviews, new List<GoodReview> { review });
            }
        }
    }
}
