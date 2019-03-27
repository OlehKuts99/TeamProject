using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Classes;
using Store.Classes.UnitOfWork;
using Store.Models;
using Store.ViewModels;

namespace Store.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProducerController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public ProducerController(AppDbContext appDbContext)
        {
            this.unitOfWork = new UnitOfWork(appDbContext);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(unitOfWork.Producers.GetAll().ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public  async Task<IActionResult> Create(CreateProducerView model)
        {
            if (ModelState.IsValid)
            {
                Producer producer = new Producer
                {
                    Name = model.Name,
                    Phone = model.Phone,
                    Email = model.Email,
                    WebSite = model.WebSite
                };
                await unitOfWork.Producers.Create(producer);
                await unitOfWork.SaveAsync();

                return RedirectToAction("Index", "Producer");

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Producer producer = await unitOfWork.Producers.Get(id);
           
            if (producer == null)
            {
                return NotFound();
            }

            EditProducerView model = new EditProducerView
            {
                Id = producer.Id,
                Name = producer.Name,
                Phone = producer.Phone,
                Email = producer.Email,
                WebSite = producer.WebSite
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProducerView model)
        {
            if (ModelState.IsValid)
            {
                Producer producer = await unitOfWork.Producers.Get(model.Id);
                
                if (producer != null)
                {
                    producer.Name = model.Name;
                    producer.Phone = model.Phone;
                    producer.Email = model.Email;
                    producer.WebSite = model.WebSite;
                    unitOfWork.Producers.Update(producer);
                    await unitOfWork.SaveAsync();

                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            Producer producer = await unitOfWork.Producers.Get(id);
            if (producer != null)
            {
                await unitOfWork.Producers.Delete(id);
                await unitOfWork.SaveAsync();
            }

            return RedirectToAction("Index");
        }
        
    }
}