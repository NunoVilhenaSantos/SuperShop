using SuperShop.Web.Data.DataContext;
using SuperShop.Web.Data.Entities;

namespace SuperShop.Web.Data.Repositories;

public class CountryRepository : GenericRepository<Country>, ICountryRepository
{
    protected CountryRepository(DataContextMssql dataContextMssql) :
        base(dataContextMssql)
    {
    }
}