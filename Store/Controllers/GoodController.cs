using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Classes;
using Store.Classes.UnitOfWork;
using Store.Models;
using Store.ViewModels;

namespace Store.Controllers
{
    public class GoodController:Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UnitOfWork unitOfWork;

        public GoodController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, AppDbContext appDbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.unitOfWork = new UnitOfWork(appDbContext);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(unitOfWork.Goods.GetAll().ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGoodView model)
        {
            if (ModelState.IsValid)
            {
                Good Good = new Good
                {
                    Name = model.Name,
                    Specification = model.Specification,
                    PhotoUrl = model.PhotoUrl,
                    YearOfManufacture = model.YearOfManufacture,
                    WarrantyTerm = model.WarrantyTerm,
                    ProducerId = model.Producer.Id,
                    Producer = model.Producer,
                    Price = model.Price,
                    Type = model.Type,
                    Count = model.Count
                };


                if (await roleManager.FindByNameAsync("Good") == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("Good"));
                }

                await unitOfWork.Goods.Create(Good);
                await unitOfWork.SaveAsync();

                return RedirectToAction("Index", "Good");

            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Good good = await unitOfWork.Goods.Get(id);
            
            var allRoles = roleManager.Roles.ToList();

            if (good == null)
            {
                return NotFound();
            }

            EditGoodView model = new EditGoodView
            {
                Id = good.Id,
                Name = good.Name,
                Specification = good.Specification,
                PhotoUrl = good.PhotoUrl,
                YearOfManufacture = good.YearOfManufacture,
                WarrantyTerm = good.WarrantyTerm,
                ProducerId = good.Producer.Id,
                Producer = good.Producer,
                Price = good.Price,
                Type = good.Type,
                Count = good.Count
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditGoodView model, List<string> roles)
        {
            if (ModelState.IsValid)
            {
                Good good = await unitOfWork.Goods.Get(model.Id);

                if (good != null)
                {
                    good.Name = model.Name;
                    good.Specification = model.Specification;
                    good.PhotoUrl = model.PhotoUrl;
                    good.YearOfManufacture = model.YearOfManufacture;
                    good.WarrantyTerm = model.WarrantyTerm;
                    good.ProducerId = model.Producer.Id;
                    good.Producer = model.Producer;
                    good.Price = model.Price;
                    good.Type = model.Type;
                    good.Count = model.Count;

                    unitOfWork.Goods.Update(good);
                    await unitOfWork.SaveAsync();
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            Good good = await unitOfWork.Goods.Get(id);

            if ( good != null)
            {
                await unitOfWork.Goods.Delete(id);
                await unitOfWork.SaveAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Find()
        {
            return View();
        }
        

        public IActionResult FindResult()
        {
            var Goods = TempData.Get<List<Good>>("list");

            if (Goods == null)
            {
                return RedirectToAction("Find");
            }

            return View(Goods);
        }
    }
}
