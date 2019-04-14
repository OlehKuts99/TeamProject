using DAL.Classes.UnitOfWork;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Controllers
{
    public class CartController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public CartController(AppDbContext appDbContext)
        {
            this.unitOfWork = new UnitOfWork(appDbContext);
        }

        [Authorize(Roles="customer")]
        [HttpGet]
        public async Task<IActionResult> ShowCart()
        {
            int customerId = unitOfWork.Customers.GetAll().Where(c => c.Email == User.Identity.Name)
                .FirstOrDefault().Id;
            Customer customer = await unitOfWork.Customers.Get(customerId);
            var goodCarts = customer.Cart.Goods;
            var goods = new List<Good>();

            foreach (var good in goodCarts)
            {
                good.Good = await unitOfWork.Goods.Get(good.GoodId);
                goods.Add(good.Good);
            }

            ViewBag.CommonPrice = goods.Sum(g => g.Price);

            return View(goods);
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

            return RedirectToAction("ShowGood", "GoodPage", new { goodId = id });
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

            return RedirectToAction("ShowCart", "Cart");
        }
    }
}
