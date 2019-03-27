using Microsoft.AspNetCore.Identity;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Classes
{
    public class DataBaseInitializer
    {
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            AppDbContext appContext)
        {
            AppDbContext appDbContext = appContext;
            UnitOfWork.UnitOfWork unitOfWork = new UnitOfWork.UnitOfWork(appDbContext);
            string adminEmail = "administrator123@gmail.com";
            string adminPassword = "Admin123";

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await roleManager.FindByNameAsync("customer") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("customer"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                ApplicationUser admin = new ApplicationUser { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

            if (appDbContext != null)
            {
                if (unitOfWork.Storages.GetAll().Where(s => s.Street == "Horodotska, 17").Count() == 0)
                { 
                    await unitOfWork.Storages.Create(new Storage() { City = "Lviv", Street = "Horodotska, 17" });
                    await unitOfWork.Storages.Create(new Storage() { City = "Kiev", Street = "Kachalova, 54" });
                }

                if (unitOfWork.Producers.GetAll().Where(p => p.Name == "Impression").Count() == 0)
                {
                    await unitOfWork.Producers.Create(new Producer()
                    {
                        Name = "Impression",
                        Phone = 443230303,
                        Email = "info@impression.ua",
                        WebSite = "https://impression.ua/"
                    });

                    unitOfWork.SaveAsync();
                }

                if (unitOfWork.Goods.GetAll().Where(g => g.Name == "ImPAD B701").Count() == 0)
                {
                    await unitOfWork.Goods.Create(
                    new Good()
                    {
                        Name = "ImPAD B701",
                        Specification = "Laptop Impression ImPAD B701 7' IPS(1024x600) / " +
                        "Spreadtrum SC7731C to 1.2 Ghz / RAM 1 Gb / Memory 8 Gb / 2G/3G",
                        PhotoUrl = "test",
                        YearOfManufacture = 2017,
                        WarrantyTerm = 12,
                        Producer = unitOfWork.Producers.GetAll().Where(p => p.Name == "Impression").First(),
                        Price = 1599,
                        Type = "Laptop",
                        Count = 450
                    });

                    await unitOfWork.SaveAsync();
                }
            }
        }
    }
}
