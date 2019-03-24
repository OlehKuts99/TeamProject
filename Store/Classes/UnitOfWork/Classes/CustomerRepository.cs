using System.Collections.Generic;
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
            await this.applicationContext.AddAsync(item);
        }

        public async Task Delete(int id)
        {
            Customer customer = await applicationContext.Customers.FindAsync(id);

            if (customer != null)
            {
                applicationContext.Remove(customer);
            }
        }

        public async Task<Customer> Get(int id)
        {
            Customer customer = await applicationContext.Customers.FindAsync(id);

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
    }
}
