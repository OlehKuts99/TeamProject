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
            var orders = unitOfWork.Orders.GetAll().ToList();
            var customers = unitOfWork.Customers.GetAll().ToList();
            foreach (var order in orders)
            {
               order.Customer = customers.Where(p => p.Id == order.CustomerId).First();
            }

            return View(orders);
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
                        order.Customer = unitOfWork.Customers.GetAll().Where(p => p.Id == order.CustomerId).First();
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