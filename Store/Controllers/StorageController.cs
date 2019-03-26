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
    public class StorageController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UnitOfWork unitOfWork;

        public StorageController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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
            return View(unitOfWork.Storages.GetAll().ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateStorageView model)
        {
            if (ModelState.IsValid)
            {
                Storage storage = new Storage
                {
                    City = model.City,
                    Street = model.Street,
                };

                await unitOfWork.Storages.Create(storage);
                await unitOfWork.SaveAsync();

                return RedirectToAction("Index", "Storage");

            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Storage storage = await unitOfWork.Storages.Get(id);
            if (storage == null)
            {
                return NotFound();
            }

            EditSorageView model = new EditSorageView
            {
                Id = storage.Id,
                City = storage.City,
                Street = storage.Street,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSorageView model)
        {
            if (ModelState.IsValid)
            {
                Storage storage = await unitOfWork.Storages.Get(model.Id);

                if (storage != null)
                {
                    storage.City = model.City;
                    storage.Street = model.Street;
                    unitOfWork.Storages.Update(storage);
                    await unitOfWork.SaveAsync();
                }
            }
            return RedirectToAction("Index","Storage");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            Storage storage = await unitOfWork.Storages.Get(id);

            if (storage != null)
            {
                await unitOfWork.Storages.Delete(id);
                await unitOfWork.SaveAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
