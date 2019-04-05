using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.Classes.UnitOfWork.Interfaces;
using Store.Models;

namespace Store.Classes.UnitOfWork.Classes
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly AppDbContext applicationContext;

        public CustomerRepository(AppDbContext appDbContext)
        {
            this.applicationContext = appDbContext;
        }

        public async Task Create(Customer item)
        {
            Cart cart = new Cart
            {
                CustomerId = item.Id
            };

            await this.applicationContext.Carts.AddAsync(cart);
            await this.applicationContext.Customers.AddAsync(item);
        }

        public async Task Delete(int id)
        {
            Customer customer = await applicationContext.Customers.FindAsync(id);

            if (customer != null)
            {
                Cart cart = applicationContext.Carts.Where(c => c.CustomerId == customer.Id).First();
                applicationContext.Carts.Remove(cart);
                applicationContext.Customers.Remove(customer);
            }
        }

        public async Task<Customer> Get(int id)
        {
            Customer customer = await applicationContext.Customers.FindAsync(id);
            customer.Cart = applicationContext.Carts.Where(c => c.CustomerId == customer.Id).FirstOrDefault();

            if (customer.Cart == null)
            {
                Cart cart = new Cart
                {
                    CustomerId = customer.Id
                };

                customer.Cart = cart;
                await this.applicationContext.Carts.AddAsync(cart);
            }

            return customer;
        }

        public IEnumerable<Customer> GetAll()
        {
            return applicationContext.Customers;
        }

        public void Update(Customer item)
        {
            applicationContext.Entry(item).State = EntityState.Modified;
        }

        public void AddToCart(Good good, Customer customer)
        {
            customer.Cart.Goods.Add(new GoodCart { Good = good, Cart = customer.Cart });
        }
    }
}
