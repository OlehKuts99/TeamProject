using DAL.Classes;
using DAL.Classes.UnitOfWork;
using DAL.Interfaces;
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
                    StarCount = Convert.ToInt32(Request.Form["mark"])
                };

                Good good = await unitOfWork.Goods.Get(id);

                await unitOfWork.Goods.AddReview(review, good);
                await unitOfWork.SaveAsync();
            }

            return RedirectToAction("ShowGood", new { goodId = id });
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

            return RedirectToAction("ShowGood", new { goodId = id });
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

        public async Task<IActionResult> DeleteReview(int id)
        {
            GoodReview review = unitOfWork.Goods.GetAllReviews().Where(r => r.Id == id).First();
            int goodId = review.GoodId;

            unitOfWork.Goods.DeleteReview(review);
            await unitOfWork.SaveAsync();

            return RedirectToAction("ShowGood", new { goodId });
        }

        public async Task<IActionResult> EditReview(int goodId)
        {
            Good good = await unitOfWork.Goods.Get(goodId);
            GoodReview review = unitOfWork.Goods.GetReviews(good.Id)
                .Where(r => r.Id == Convert.ToInt32(Request.Form["reviewId"])).First();
            IUpdater<GoodReview> reviewUpdater = new Updater<GoodReview>(unitOfWork.GetContext());

            review.Message = Request.Form["newMessage"];
            review.StarCount = this.CheckNewStarCount(Request.Form["newStarCount"]);
            review.Date = DateTime.Now;
            reviewUpdater.Update(review);
            await unitOfWork.SaveAsync();

            return RedirectToAction("ShowGood", new { goodId });
        }

        private int CheckNewStarCount(string starCount)
        {
            int result = Convert.ToInt32(starCount);

            if (result > 5)
            {
                result = 5;
            } 

            if (result < 0)
            {
                result = 0;
            }

            return result;
        }
    }
}
