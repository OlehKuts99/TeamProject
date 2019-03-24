using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.Classes.UnitOfWork.Interfaces;
using Store.Models;

namespace Store.Classes.UnitOfWork.Classes
{
    public class GoodRepository : IRepository<Good>
    {
        private readonly AppDbContext applicationContext;

        public GoodRepository(AppDbContext appDbContext)
        {
            this.applicationContext = appDbContext;
        }

        public async Task Create(Good item)
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

        public async Task<Good> Get(int id)
        {
            Good good = await applicationContext.Goods.FindAsync(id);

            return good;
        }

        public IEnumerable<Good> GetAll()
        {
            return applicationContext.Goods;
        }

        public void Update(Good item)
        {
            applicationContext.Entry(item).State = EntityState.Modified;
        }
    }
}
