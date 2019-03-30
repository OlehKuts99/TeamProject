using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Classes;
using Store.Classes.UnitOfWork;
using Store.Models;
using Store.ViewModels;

namespace Store.Controllers
{
    [Authorize(Roles = "admin")]
    public class GoodController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public GoodController(AppDbContext appDbContext)
        { 
            this.unitOfWork = new UnitOfWork(appDbContext);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var goods = unitOfWork.Goods.GetAll().ToList();
            var producers = unitOfWork.Producers.GetAll().ToList();

            foreach (var good in goods)
            {
                good.Producer = producers.Where(p => p.Id == good.ProducerId).First();
            }

            return View(goods);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateGoodView model = new CreateGoodView
            {
                Producers = unitOfWork.Producers.GetAll().ToList(),
                Storages = unitOfWork.Storages.GetAll().ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGoodView model, List<string> storages)
        {
            if (ModelState.IsValid)
            {
                Good good = new Good
                {
                    Name = model.Name,
                    Specification = model.Specification,
                    PhotoUrl = model.PhotoUrl,
                    YearOfManufacture = model.YearOfManufacture,
                    WarrantyTerm = model.WarrantyTerm,
                    Producer = unitOfWork.Producers.GetAll().Where(p => p.Name == Request.Form["producerSelect"]).First(),
                    Price = model.Price,
                    Type = model.Type,
                    Count = model.Count,
                };

                if (storages.Count > 0)
                {
                    foreach (var storage in storages)
                    {
                        Storage tempStorage = unitOfWork.Storages.GetAll().Where(s => s.Street == storage).First();
                        good.Storages.Add(new GoodStorage() { Good = good, Storage = tempStorage });
                    }
                }

                await unitOfWork.Goods.Create(good);
                await unitOfWork.SaveAsync();

                return RedirectToAction("Index", "Good");

            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Good good = await unitOfWork.Goods.Get(id);

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
                Price = good.Price,
                Type = good.Type,
                Count = good.Count,
                Producer = await unitOfWork.Producers.Get(good.ProducerId),
                Storages = good.Storages
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditGoodView model)
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
            var goods = HttpContext.Session.Get<List<Good>>("list");

            if (goods == null)
            {
                return RedirectToAction("Find");
            }

            return View(goods);
        }
    }
}
