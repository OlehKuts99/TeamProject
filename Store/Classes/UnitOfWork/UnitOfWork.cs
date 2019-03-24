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
