using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Classes;
using DAL.Classes.UnitOfWork;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.ViewModels;

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
            var goods = new List<OrderPart>();

            foreach (var good in customer.Cart.Goods)
            {
                good.Good = await unitOfWork.Goods.Get(good.GoodId);
                goods.Add(new OrderPart { Good = good.Good, Count = 1 });
            }

            ViewBag.CommonPrice = Convert.ToInt32(goods.Sum(g => g.Good.Price));
            HttpContext.Session.Set("goodsConfirm", goods);

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
        [HttpGet]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            Good good = await unitOfWork.Goods.Get(id);
            int customerId = unitOfWork.Customers.GetAll().Where(c => c.Email == User.Identity.Name).First().Id;
            Customer customer = await unitOfWork.Customers.Get(customerId);

            if (good != null && customer != null)
            {
                unitOfWork.Customers.RemoveFromCart(good, customer);
                await unitOfWork.SaveAsync();
            }

            return RedirectToAction("ShowCart", "Cart");
        }

        [Authorize(Roles = "customer")]
        [HttpPost]
        public async Task<IActionResult> ConfirmGoods(List<string> goodCount)
        {
            int customerId = unitOfWork.Customers.GetAll().Where(c => c.Email == User.Identity.Name).First().Id;
            Customer customer = await unitOfWork.Customers.Get(customerId);
            var goods = HttpContext.Session.Get<List<OrderPart>>("goodsConfirm");
            var modelGoods = new List<OrderPart>();

            if (goods == null)
            {
                return RedirectToAction("ShowCart", "Cart");
            }

            for (int i = 0; i < goodCount.Count; i++)
            {
                if (goodCount[i] == "0")
                {
                    continue;
                }

                modelGoods.Add(new OrderPart { Good = goods[i].Good, Count = Convert.ToInt32(goodCount[i])});
            }

            ConfirmOrderView model = new ConfirmOrderView
            {
                Customer = customer,
                Goods = modelGoods,
                Storages = unitOfWork.Storages.GetAll().ToList(),
                Count = Convert.ToInt32(Request.Form["goodCommonCount"]),
                CommonPrice = Convert.ToInt32(Request.Form["commonPrice"])
            };

            return View("ConfirmOrder", model);
        }
    }
}
