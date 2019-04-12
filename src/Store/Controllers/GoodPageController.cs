using DAL.Classes.UnitOfWork;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Controllers
{
    public class GoodPageController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public GoodPageController(AppDbContext appDbContext)
        {
            this.unitOfWork = new UnitOfWork(appDbContext);
        }

        public async Task<IActionResult> ShowGood(int goodId)
        {
            Good good = await unitOfWork.Goods.Get(goodId);
            good.Producer = await unitOfWork.Producers.Get(good.ProducerId);

            return View(good);
        }

        [Authorize(Roles = "customer")]
        [HttpPost]
        public async Task<IActionResult> LeaveReview(int id, string reviewArea)
        {
            Customer customer = unitOfWork.Customers.GetAll().Where(c => c.Email == User.Identity.Name).First();

            if (customer != null)
            {
                GoodReview review = new GoodReview
                {
                    Customer = customer,
                    Date = DateTime.Now,
                    Message = reviewArea,
                    StarCount = 0
                };

                Good good = await unitOfWork.Goods.Get(id);

                await unitOfWork.Goods.AddReview(review, good);
                await unitOfWork.SaveAsync();
            }

            return View();
        }

        [Authorize(Roles = "customer")]
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            Good good = await unitOfWork.Goods.Get(id);
            int customerId = unitOfWork.Customers.GetAll().Where(c => c.Email == User.Identity.Name).First().Id;
            Customer customer = await unitOfWork.Customers.Get(customerId);

            if (good != null && customer != null)
            {
                unitOfWork.Customers.AddToCart(good, customer);
                await unitOfWork.SaveAsync();
            }
            return RedirectToAction("ShowGood", new { goodid = id });
        }

        [Authorize(Roles = "customer")]
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int goodid)
        {
            Good good = await unitOfWork.Goods.Get(goodid);
            int customerId = unitOfWork.Customers.GetAll().Where(c => c.Email == User.Identity.Name).First().Id;
            Customer customer = await unitOfWork.Customers.Get(customerId);

            if (good != null && customer != null)
            {
                unitOfWork.Customers.RemoveFromCart(good, customer);
                await unitOfWork.SaveAsync();
            }
            return RedirectToAction("ShowCart", "Account");
        }
    }
}
