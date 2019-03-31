using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Store.Classes;
using Store.Classes.UnitOfWork;
using Store.Models;
using Store.ViewModels;

namespace Store.Controllers
{
    [Authorize(Roles = "admin")]
    public class OrderController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public OrderController(AppDbContext appDbContext)
        {
            this.unitOfWork = new UnitOfWork(appDbContext);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(unitOfWork.Orders.GetAll().ToList());
        }

        [HttpGet]
        public IActionResult Find()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Find(FindOrderView model)
        {
            List<Order> orders = new List<Order>();
            if (ModelState.IsValid)
            {
                var allOrders = unitOfWork.Orders.GetAll().ToList();
                foreach (var order in allOrders)
                {
                    bool addToResult = false;
                    if (model.Id == order.Id || model.OrderDate == order.OrderDate)
                    {
                        addToResult = true;
                    }
                    if (addToResult)
                    {
                        orders.Add(order);
                    }
                }

                HttpContext.Session.Set("list", orders);

                return RedirectToAction("FindResult", "Order");
            }
            return View(model);
        }

        public IActionResult FindResult()
        {
            var orders = HttpContext.Session.Get<List<Order>>("list");

            if (orders == null)
            {
                return RedirectToAction("Find");
            }

            return View(orders);
        }
    }
}