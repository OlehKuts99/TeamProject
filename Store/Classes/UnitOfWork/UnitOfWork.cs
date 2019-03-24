using System;
using System.Threading.Tasks;
using Store.Classes.UnitOfWork.Classes;
using Store.Models;

namespace Store.Classes.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        private readonly AppDbContext applicationContext;
        private CustomerRepository customerRepository;
        private GoodRepository goodRepository;
        private OrderRepository _orderRepository;
        private bool disposed = false;

        public UnitOfWork(AppDbContext appDbContext)
        {
            this.applicationContext = appDbContext;
        }

        public CustomerRepository Customers
        {
            get
            {
                if (customerRepository == null)
                {
                    customerRepository = new CustomerRepository(this.applicationContext);
                }

                return customerRepository;
            }
        }

        public GoodRepository Goods
        {
            get
            {
                if (goodRepository == null)
                {
                    goodRepository = new GoodRepository(this.applicationContext);
                }

                return goodRepository;
            }
        }

        public OrderRepository Orders
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(this.applicationContext);
                }

                return _orderRepository;
            }
        }


        public async Task SaveAsync()
        {
            await applicationContext.SaveChangesAsync();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    applicationContext.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
