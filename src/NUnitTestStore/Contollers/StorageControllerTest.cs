using DAL.Classes.UnitOfWork;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using Store.Controllers;
using Store.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    public class StorageControllerTest
    {
        DbContextOptions<AppDbContext> options;
        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database").Options;
        }

        [Test]
        public void Index_Change_Data()
        {
            // Arrange
            var storages = new List<Storage>
            {
                new Storage { Id = 1 },
                new Storage { Id = 2 },
                new Storage { Id = 3 },
            };

            using (var context = new AppDbContext(options))
            {
                var controller = new StorageController(context);
                foreach (var storage in storages)
                {
                    context.Storages.Add(storage);
                }
                context.SaveChanges();

                // Act
                controller.Index();

                // Assert
                Assert.AreEqual(storages.Count, context.Storages.Count());
                Assert.AreEqual(storages[0].Id, context.Storages.First().Id);
            }
        }

        [Test]
        public async Task Add_Storage_To_DataBase()
        {
            // Arrange
            using (var context = new AppDbContext(options))
            {
                var controller = new StorageController(context);
                var storage = new Storage() { Id = 1, City = "Lviv", Street = "Rubchaka, 56" };
                var model = new CreateStorageView() { City = storage.City, Street = storage.Street };

                // Act
                await controller.Create(model);

                // Assert
                Assert.AreEqual(1, context.Storages.Count());
                Assert.AreEqual(model.City, context.Storages.Single().City);
            }
        }

        [Test]
        public async Task Delete_Storage_From_Database()
        {
            using (var context = new AppDbContext(options))
            {
                // Arrange
                var controller = new StorageController(context);
                var storage = new Storage() { Id = 1, City = "Lviv", Street = "Rubchaka, 56" };
                var model = new CreateStorageView() { City = storage.City, Street = storage.Street };

                // Act
                await controller.Create(model);
                await controller.Delete(storage.Id);

                // Assert
                Assert.AreEqual(0, context.Storages.Count());
            }
        }

        [Test]
        public async Task Edit_Some_Storage()
        {
            using (var context = new AppDbContext(options))
            {
                // Arrange
                var controller = new StorageController(context);
                var storage = new Storage() { Id = 1, City = "Lviv", Street = "Rubchaka, 56" };
                var model = new CreateStorageView() { City = storage.City, Street = storage.Street };
                var editModel = new EditSorageView() {  Id = storage.Id, City = "Kiev", Street = storage.Street };

                // Act
                await controller.Create(model);
                await controller.Edit(editModel);

                // Assert
                Assert.AreEqual(1, context.Storages.Count());
                Assert.AreEqual(model.Street, context.Storages.Single().Street);
                Assert.AreNotEqual(editModel.City, model.City);
                Assert.AreEqual(editModel.City, context.Storages.Single().City);
            }
        }
    }
}