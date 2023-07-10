using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.DataContext;
using SuperShop.Web.Data.Entities;

namespace SuperShop.Web.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class, IEntity
{
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;


    protected GenericRepository(
        DataContextMsSql dataContextMsSql,
        DataContextMySql dataContextMySql,
        DataContextSqLite dataContextSqLite
    )
    {
        _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;
    }


    public IQueryable<T> GetAll()
    {
        return _dataContextMsSql.Set<T>()
            .AsNoTracking().AsQueryable();
    }


    public async Task<T> GetByIdAsync(int id)
    {
        return await _dataContextMsSql.Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }


    public async Task CreateAsync(T entity)
    {
        await _dataContextMsSql.Set<T>().AddAsync(entity);

        await SaveAllAsync();

        // return entity;
    }


    public async Task UpdateAsync(T entity)
    {
        _dataContextMsSql.Set<T>().Update(entity);

        await SaveAllAsync();

        // return entity;
    }


    public async Task DeleteAsync(T entity)
    {
        _dataContextMsSql.Set<T>().Remove(entity);

        await SaveAllAsync();

        // return entity;
    }


    public async Task<bool> ExistAsync(int id)
    {
        return await _dataContextMsSql.Set<T>()
            .AsNoTracking()
            .AnyAsync(e => e.Id == id);
    }


    private async Task<bool> SaveAllAsync()
    {
        return await _dataContextMsSql.SaveChangesAsync() > 0;
    }
}