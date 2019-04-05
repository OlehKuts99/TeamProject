using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Store.Classes.UnitOfWork;
using Store.Models;

namespace Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public HomeController(AppDbContext appDbContext)
        {
            this.unitOfWork = new UnitOfWork(appDbContext);
        }

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

        public IActionResult NameSearch(string goodType)
        {
            var goods = unitOfWork.Goods.GetAll().ToList().Where(p=>p.Type==goodType);
            var producers = unitOfWork.Producers.GetAll().ToList();

            foreach (var good in goods)
            {
                good.Producer = producers.Where(p => p.Id == good.ProducerId).First();
            }

            return View("Index",goods);
        }

        public async Task<IActionResult> GoodPage(int goodId)
        {
            return View(await unitOfWork.Goods.Get(goodId));
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
