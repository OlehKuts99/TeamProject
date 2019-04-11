using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Store.ViewModels;
using DAL.Classes.UnitOfWork;
using DAL.Models;
using DAL.Classes;

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
            FindOrderView model = new FindOrderView
            {
                OrderStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList(),
                SelectedStatuses = new List<OrderStatus>()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Find(FindOrderView model, List<OrderStatus> statuses)
        {
            List<Order> orders = new List<Order>();
            if (ModelState.IsValid)
            {
                var allOrders = unitOfWork.Orders.GetAll().ToList();
                foreach (var order in allOrders)
                {
                    bool addToResult = true;

                    if (model.Id == null && model.OrderDate == null && statuses.Count == 0)
                    {
                        addToResult = false;
                    }

                    if (model.Id != null && order.Id != model.Id)
                    {
                        addToResult = false;
                    }

                    if (model.OrderDate != null && ((DateTime)model.OrderDate).Date != order.OrderDate.Date)
                    {
                        addToResult = false;
                    }

                    if (statuses.Count > 0 && !statuses.Contains(order.OrderStatus))
                    {
                        addToResult = false;
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

        [HttpGet]
        public async Task<IActionResult> ShowGoods(int id)
        {
            Order order = await unitOfWork.Orders.Get(id);
            List<Good> goods = new List<Good>();
            
            foreach (var item in order.Products)
            {
                goods.Add(await unitOfWork.Goods.Get(item.GoodId));
            }

            ViewBag.OrderId = order.Id;

            return View(goods);
        }

        [HttpGet]
        public async Task<IActionResult> ShowCustomer(int id)
        {
            Order order = await unitOfWork.Orders.Get(id);
            Customer customer = await unitOfWork.Customers.Get(order.CustomerId);

            ViewBag.OrderId = order.Id;

            return View(customer);
        }
    }
}