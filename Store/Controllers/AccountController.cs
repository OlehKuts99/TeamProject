using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Models;
using Store.ViewModels;

namespace Store.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AppDbContext appContext;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            AppDbContext appDbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.appContext = appDbContext;
        }

        /// <summary>
        /// Get method to show registration form.
        /// </summary>
        /// <returns>Register view that contains registration form.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult RegisterView()
        {
            return View();
        }
        /// <summary>
        /// Post method that will be runned after pushing on the button in registration form. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>If operation is success it redirects to main page, otherwise it will return the same page.</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterView model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = string.Join(' ', model.FirstName, model.SecondName),
                    CreateDate = DateTime.Now,
                    UpdateTime = DateTime.Now
                };

                Customer customer = new Customer
                {
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    Phone = model.Phone,
                    Email = model.Email
                };

                var addResult = await userManager.CreateAsync(user, model.Password);

                if (addResult.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    await appContext.Customers.AddAsync(customer);
                    await appContext.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in addResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }
    }
}
