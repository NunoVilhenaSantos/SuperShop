using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Data
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class, IEntity
    {
        private readonly DataContext _dataContext;

        protected GenericRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public IQueryable<T> GetAll()
        {
            return _dataContext.Set<T>()
                .AsNoTracking().AsQueryable();
        }


        public async Task<T> GetByIdAsync(int id)
        {
            return await _dataContext.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }


        public async Task<T> CreateAsync(T entity)
        {
            await _dataContext.Set<T>().AddAsync(entity);

            await SaveAllAsync();

            return entity;
        }


        public async Task<T> UpdateAsync(T entity)
        {
            _dataContext.Set<T>().Update(entity);

            await SaveAllAsync();

            return entity;
        }


        public async Task<T> DeleteAsync(T entity)
        {
            _dataContext.Set<T>().Remove(entity);

            await SaveAllAsync();

            return entity;
        }


        public async Task<bool> ExistAsync(int id)
        {
            return await _dataContext.Set<T>()
                .AsNoTracking()
                .AnyAsync(e => e.Id == id);
        }


        private async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}