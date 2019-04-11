using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Classes.UnitOfWork.Interfaces;
using DAL.Models;

namespace DAL.Classes.UnitOfWork.Classes
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Order item)
        {
            await this._dbContext.Orders.AddAsync(item);
        }

        public async Task Delete(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);

            if (order != null)
            {
                _dbContext.Orders.Remove(order);
            }
        }

        public async Task<Order> Get(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            order.Products = _dbContext.GoodOrder.Where(g => g.OrderId == order.Id).ToList();
            order.Customer = await _dbContext.Customers.FindAsync(order.CustomerId);

            return order;
        }

        public IEnumerable<Order> GetAll()
        {
            return _dbContext.Orders;
        }

        public void Update(Order item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }
    }
}