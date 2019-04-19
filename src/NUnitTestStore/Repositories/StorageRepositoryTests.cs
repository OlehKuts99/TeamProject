using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Classes.UnitOfWork.Classes;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace NUnitTestStore.Repositories
{
    [TestFixture]
    class StorageRepositoryTests
    {
        DbContextOptions<AppDbContext> options;
        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: "InMemoryDatabase").Options;
        }

        [Test]
        public async Task CreateTest()
        {
            //Arrange
            using (var context = new AppDbContext(options))
            {
                var storage = new Storage { Id = 1 };
                var repo = new StorageRepository(context);

                //Act
                await repo.Create(storage);
                var expectedResult = 1;
                var actualResult = context.Storages.Local.Count;

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
                var storage = new Storage { Id = 1 };
                var repo = new StorageRepository(context);

                //Act
                context.Storages.Add(storage);
                await repo.Delete(storage.Id);
                var expectedResult = 0;
                var actualResult = context.Storages.Local.Count;

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
                var storage = new Storage { Id = 1 };
                var repo = new StorageRepository(context);

                //Act
                context.Storages.Add(storage);
                var expectedResult = await repo.Get(storage.Id);
                var actualResult = storage;

                //Assert
                Assert.AreEqual(expectedResult, actualResult);
            }
        }

        [Test]
        public void GetAllTest()
        {
            //Arrange
            using (var context = new AppDbContext(options))
            {
                var reviews = new List<GoodReview> { new GoodReview { Id = 1, GoodId = 1 },
                    new GoodReview { Id = 2, GoodId = 1 }, new GoodReview { Id = 3, GoodId = 1 } };
                var repo = new StorageRepository(context);

                //Act
                foreach (var review in reviews)
                {
                    context.Reviews.Add(review);
                }
                var expectedResult = repo.GetAll();
                var actualResult = context.Reviews;

                //Assert
                Assert.AreEqual(expectedResult, actualResult);
            }
        }

        [Test]
        public void UpdateTest()
        {
            //Arrange
            using (var context = new AppDbContext(options))
            {
                var storage = new Storage { Id = 1 };
                var repo = new StorageRepository(context);

                //Act
                repo.Update(storage);
                var actualResult = context.Entry(storage).State;
                var expectedResult = EntityState.Modified;

                //Assert
                Assert.AreEqual(expectedResult, actualResult);
            }
        }
    }
}
