using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.Classes.UnitOfWork.Interfaces;
using Store.Models;

namespace Store.Classes.UnitOfWork.Classes
{
    public class StorageRepository : IRepository<Storage>
    {
        private readonly AppDbContext appContext;

        public StorageRepository(AppDbContext appDbContext)
        {
            this.appContext = appDbContext;
        }

        public async Task Create(Storage item)
        {
            await this.appContext.Storages.AddAsync(item);
        }

        public async Task Delete(int id)
        {
            Storage storage = await appContext.Storages.FindAsync(id);

            if (storage != null)
            {
                appContext.Storages.Remove(storage);
            }
        }

        public async Task<Storage> Get(int id)
        {
            Storage storage = await appContext.Storages.FindAsync(id);
            storage.Products = appContext.GoodStorage.Where(g => g.StorageId == id).ToList();

            return storage;
        }

        public IEnumerable<Storage> GetAll()
        {
            return appContext.Storages;
        }

        public void Update(Storage item)
        {
            appContext.Entry(item).State = EntityState.Modified;
        }
    }
}
