using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.DataContext;
using SuperShop.Web.Data.Entities;

namespace SuperShop.Web.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class, IEntity
{
    private readonly DataContextMssql _dataContextMssql;

    protected GenericRepository(DataContextMssql dataContextMssql)
    {
        _dataContextMssql = dataContextMssql;
    }


    public IQueryable<T> GetAll()
    {
        return _dataContextMssql.Set<T>()
            .AsNoTracking().AsQueryable();
    }


    public async Task<T> GetByIdAsync(int id)
    {
        return await _dataContextMssql.Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }


    public async Task CreateAsync(T entity)
    {
        await _dataContextMssql.Set<T>().AddAsync(entity);

        await SaveAllAsync();

        // return entity;
    }


    public async Task UpdateAsync(T entity)
    {
        _dataContextMssql.Set<T>().Update(entity);

        await SaveAllAsync();

        // return entity;
    }


    public async Task DeleteAsync(T entity)
    {
        _dataContextMssql.Set<T>().Remove(entity);

        await SaveAllAsync();

        // return entity;
    }


    public async Task<bool> ExistAsync(int id)
    {
        return await _dataContextMssql.Set<T>()
            .AsNoTracking()
            .AnyAsync(e => e.Id == id);
    }


    private async Task<bool> SaveAllAsync()
    {
        return await _dataContextMssql.SaveChangesAsync() > 0;
    }
}