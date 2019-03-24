using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Classes.UnitOfWork;
using Store.Models;
using Store.ViewModels;

namespace Store.Controllers
{
    public class CustomerController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UnitOfWork unitOfWork;

        public CustomerController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, AppDbContext appDbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.unitOfWork = new UnitOfWork(appDbContext);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(unitOfWork.Customers.GetAll().ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerView model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.FirstName + model.SecondName,
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
                    if (await roleManager.FindByNameAsync("customer") == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole("customer"));
                    }

                    await userManager.AddToRoleAsync(user, "customer");
                    await unitOfWork.Customers.Create(customer);
                    await unitOfWork.SaveAsync();

                    return RedirectToAction("Index", "Customer");
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Customer customer = await unitOfWork.Customers.Get(id);

            if (customer == null)
            {
                return NotFound();
            }

            EditCustomerView model = new EditCustomerView
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                SecondName = customer.SecondName,
                Phone = customer.Phone,
                Email = customer.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCustomerView model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = await unitOfWork.Customers.Get(model.Id);
                ApplicationUser user = await userManager.FindByEmailAsync(customer.Email);

                if (user != null && customer != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.FirstName + model.SecondName;
                    user.UpdateTime = DateTime.Now;

                    customer.FirstName = model.FirstName;
                    customer.SecondName = model.SecondName;
                    customer.Phone = model.Phone;
                    customer.Email = model.Email;

                    unitOfWork.Customers.Update(customer);
                    await unitOfWork.SaveAsync();

                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            Customer customer = await unitOfWork.Customers.Get(id);
            ApplicationUser user = await userManager.FindByEmailAsync(customer.Email);

            if (user != null && customer != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                await unitOfWork.Customers.Delete(id);
                await unitOfWork.SaveAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
