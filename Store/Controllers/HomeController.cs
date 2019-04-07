using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Store.Classes;
using Store.Classes.UnitOfWork;
using Store.Models;
using Store.ViewModels;

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
            var model = new FindRangeInMainView(unitOfWork);
            var goods = unitOfWork.Goods.GetAll().ToList();
            var producers = unitOfWork.Producers.GetAll().ToList();

            foreach (var good in goods)
            {
                good.Producer = producers.Where(p => p.Id == good.ProducerId).First();
            }

            HttpContext.Session.Set("filter", model);
            HttpContext.Session.Set("goods", goods);

            model.List = goods;
            return View(model);
        }

        public IActionResult Filter(FindRangeInMainView model)
        {
            var goods = new List<Good>();
            var resultModel = new FindRangeInMainView(unitOfWork);
            var allGoods = unitOfWork.Goods.GetAll().ToList();
            var producers = unitOfWork.Producers.GetAll().ToList();

            foreach (var good in allGoods)
            {
                good.Reviews = unitOfWork.Goods.GetReviews(good.Id);
                good.Producer = producers.Where(p => p.Id == good.ProducerId).First();
            }

            model.GoodView.Type = resultModel.Types.Where(t => t == Request.Form["typeSelect"]).First();
            resultModel.ChoosenType = model.GoodView.Type;
            model.Types = resultModel.Types;

            foreach (var good in allGoods)
            {
                bool addToResult = true;

                if (model.GoodView.Name != null && good.Name != model.GoodView.Name)
                {
                    addToResult = false;
                }

                if (model.GoodView.YearOfManufacture != null && good.YearOfManufacture != model.GoodView.YearOfManufacture)
                {
                    addToResult = false;
                }

                if (model.GoodView.ProducerName != null && good.Producer.Name != model.GoodView.ProducerName)
                {
                    addToResult = false;
                }

                if (model.GoodView.EndPrice - model.GoodView.StartPrice != 0 && good.Price < model.GoodView.StartPrice ||
                    good.Price > model.GoodView.EndPrice)
                {
                    addToResult = false;
                }

                if (model.GoodView.Type != null && good.Type != model.GoodView.Type)
                {
                    if (model.GoodView.Type != "All")
                    {
                        addToResult = false;
                    }
                }

                if (model.GoodView.WarrantyTerm != null && good.WarrantyTerm != model.GoodView.WarrantyTerm)
                {
                    addToResult = false;
                }

                if (addToResult)
                {
                    goods.Add(good);
                }
            }

            HttpContext.Session.Set("filter", model);
            HttpContext.Session.Set("goods", goods);

            resultModel.List = goods;
            return View("Index", resultModel);
        }

        public IActionResult TypeSearch(string goodType)
        {
            var model = new FindRangeInMainView(unitOfWork);
            var goods = unitOfWork.Goods.GetAll().ToList().Where(p=>p.Type==goodType);
            var producers = unitOfWork.Producers.GetAll().ToList();

            foreach (var good in goods)
            {
                good.Producer = producers.Where(p => p.Id == good.ProducerId).First();
            }

            HttpContext.Session.Set("filter", model);
            HttpContext.Session.Set("goods", goods);
            model.List = goods;
            return View("Index", model);
        }

        public async Task<IActionResult> GoodPage(int goodId)
        {
            Good good = await unitOfWork.Goods.Get(goodId);
            good.Producer = await unitOfWork.Producers.Get(good.ProducerId);

            return View(good);
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

        public IActionResult Sort()
        {
            var model = HttpContext.Session.Get<FindRangeInMainView>("filter");
            var goods = HttpContext.Session.Get<List<Good>>("goods");

            if (model == null)
            {
                model = new FindRangeInMainView(unitOfWork);
            }

            if (goods == null)
            {
                goods = unitOfWork.Goods.GetAll().ToList();
            }

            Enum.TryParse(Request.Form["sortSelect"].ToString(), out SortBy temp);
            model.SortBy = temp;

            if (Request.Form["sortSelect"] == SortBy.Popularity.ToString())
            {
                foreach (var good in goods)
                {
                    good.Reviews = unitOfWork.Goods.GetReviews(good.Id);
                }

                goods = goods.OrderBy(g => g.Reviews.Count).ToList();
            }

            if (Request.Form["sortSelect"] == SortBy.PriceFromBigger.ToString())
            {
                goods = goods.OrderByDescending(g => g.Price).ToList();
            }

            if (Request.Form["sortSelect"] == SortBy.PriceFromLower.ToString())
            {
                goods = goods.OrderBy(g => g.Price).ToList();
            }

            model.List = goods;
            return View("Index", model);
        }
    }
}
