﻿using System;
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

        public IActionResult Index(int page = 1)
        {
            var filterModel = HttpContext.Session.Get<FindRangeInMainView>("filter");
            if (filterModel == null)
            {
                filterModel = new FindRangeInMainView(unitOfWork);
            }
            else
            {
                filterModel.Types = filterModel.Types.Distinct().ToList();
            }

            var goods = HttpContext.Session.Get<List<Good>>("goods");
            if (goods == null)
            {
                goods = unitOfWork.Goods.GetAll().ToList();
            }

            var items = goods.Skip((page - 1) * PageViewModel.PageSize).Take(PageViewModel.PageSize);

            HttpContext.Session.Set("filter", filterModel);
            HttpContext.Session.Set("goods", goods);

            filterModel.Goods = items;
            IndexViewModel model = new IndexViewModel
            {
                PageViewModel = new PageViewModel(goods.Count, page),
                FilterModel = filterModel
            };

            return View(model);
        }

        public IActionResult Filter(IndexViewModel model)
        {
            var goods = new List<Good>();
            var tempModel = new FindRangeInMainView(unitOfWork);
            var allGoods = unitOfWork.Goods.GetAll().ToList();

            model.FilterModel.GoodView.Type = tempModel.Types
                .Where(t => t == Request.Form["typeSelect"]).First();
            tempModel.ChoosenType = model.FilterModel.GoodView.Type;
            model.FilterModel.Types = tempModel.Types;

            foreach (var good in allGoods)
            {
                bool addToResult = true;

                if (model.FilterModel.GoodView.Name != null && good.Name != model.FilterModel.GoodView.Name)
                {
                    addToResult = false;
                }

                if (model.FilterModel.GoodView.YearOfManufacture != null 
                    && good.YearOfManufacture != model.FilterModel.GoodView.YearOfManufacture)
                {
                    addToResult = false;
                }

                if (model.FilterModel.GoodView.ProducerName != null 
                    && good.Producer.Name != model.FilterModel.GoodView.ProducerName)
                {
                    addToResult = false;
                }

                if (model.FilterModel.GoodView.EndPrice - model.FilterModel.GoodView.StartPrice != 0 
                    && good.Price < model.FilterModel.GoodView.StartPrice ||
                    good.Price > model.FilterModel.GoodView.EndPrice)
                {
                    addToResult = false;
                }

                if (model.FilterModel.GoodView.Type != null && good.Type != model.FilterModel.GoodView.Type)
                {
                    if (model.FilterModel.GoodView.Type != "All")
                    {
                        addToResult = false;
                    }
                }

                if (model.FilterModel.GoodView.WarrantyTerm != null 
                    && good.WarrantyTerm != model.FilterModel.GoodView.WarrantyTerm)
                {
                    addToResult = false;
                }

                if (addToResult)
                {
                    goods.Add(good);
                }
            }

            HttpContext.Session.Set("filter", model.FilterModel);
            HttpContext.Session.Set("goods", goods);

            return RedirectToAction("Index", new { pageNumber = 1 });
        }

        public IActionResult TypeSearch(string goodType)
        {
            var model = new FindRangeInMainView(unitOfWork);
            var goods = unitOfWork.Goods.GetAll().ToList().Where(p => p.Type == goodType);

            if (goodType == "All")
            {
                goods = unitOfWork.Goods.GetAll().ToList();
            }

            HttpContext.Session.Set("filter", model);
            HttpContext.Session.Set("goods", goods);

            return RedirectToAction("Index", new { pageNumber = 1 });
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

        [HttpPost]
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

            model.Goods = goods;
            HttpContext.Session.Set("filter", model);
            HttpContext.Session.Set("goods", goods);

            return RedirectToAction("Index", new { pageNumber = 1 });
        }
    }
}
