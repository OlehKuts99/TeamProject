using DAL.Models;
using NUnit.Framework;
using Store.Controllers;
using NSubstitute;
using Store.ViewModels;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.Classes;
using DAL.Classes.UnitOfWork;
using DAL.Classes.UnitOfWork.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Helpers;
using Microsoft.EntityFrameworkCore;
using Moq;
using DAL.Classes.UnitOfWork.Interfaces;

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
               .UseInMemoryDatabase(databaseName: "Add_writes_to_database").Options;
            repoMock = new Mock<IRepository<Good>>();
        }
        

        [Test]
        public async Task CreateTest()
        {
            //Arrange
            var good = new Good { Id = 1, Name = "Test"};
            using (var context = new AppDbContext(options))
            {
                var repo = new GoodRepository(context);

                //Act
                await repo.Create(good);
                var expectedResult = 1;
                //Assert
                Assert.AreEqual(expectedResult, context.Goods.Local.Count());
            }
        }

        [Test]
        public async Task DeleteTest()
        {
            //Arrange
            var good = new Good { Id = 1, Name = "Test" };
            using (var context = new AppDbContext(options))
            {
                UnitOfWork unitOfWork = new UnitOfWork(context);
                var repo = new GoodRepository(context);

                //Act
                var expectedResult = 0;
                context.Goods.Add(good);
                var countAfterAdding = context.Goods.Local.Count();
                await repo.Delete(good.Id);

                //Assert
                Assert.AreEqual(expectedResult, context.Goods.Local.Count());
                Assert.AreEqual(1, countAfterAdding);
            }
        }
    }
}
