using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SuperShop.Web.Data.DataContext;
using SuperShop.Web.Data.Entities;
using SuperShop.Web.Models;

namespace SuperShop.Web.Data.Repositories;

public class CountryRepository : GenericRepository<Country>, ICountryRepository
{
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;

    public CountryRepository(
        DataContextMsSql dataContextMsSql,
        DataContextMySql dataContextMySql,
        DataContextSqLite dataContextSqLite
    ) : base(dataContextMsSql, dataContextMySql, dataContextSqLite)
    {
        _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;
    }

    public IQueryable<Country> GetCountriesWithCities()
    {
        return _dataContextMsSql.Countries
            .Include(c => c.Cities)
            .OrderBy(c => c.Name);
    }

    public IEnumerable<Country> GetCountriesWithCitiesEnumerable()
    {
        return _dataContextMsSql.Countries
            .Include(c => c.Cities)
            .OrderBy(c => c.Name)
            .AsEnumerable();
    }

    public IEnumerable<Country> GetCountriesWithCitiesEnumerableNoTracking()
    {
        return _dataContextMsSql.Countries
            .Include(c => c.Cities)
            .OrderBy(c => c.Name)
            .AsNoTracking().AsEnumerable();
    }

    public IEnumerable<SelectListItem> GetComboCountries()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<SelectListItem> GetComboCities(int countryId)
    {
        throw new NotImplementedException();
    }

    public async Task<Country> GetCountryWithCitiesAsync(int id)
    {
        return await _dataContextMsSql.Countries
            .Include(c => c.Cities)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Country> GetCountryWithCitiesAsync(Country country)
    {
        return await _dataContextMsSql.Countries
            .Include(c => c.Cities)
            .FirstOrDefaultAsync(c => c.Id == country.Id);
    }

    public async Task<Country> GetCountryWithCitiesAsync(City city)
    {
        return await _dataContextMsSql.Countries
            .Include(c => c.Cities
                .Where(ci => ci.Id == city.Id))
            .FirstOrDefaultAsync();
    }

    public async Task<City> GetCityAsync(int id)
    {
        return await _dataContextMsSql.Cities.FindAsync(id);
    }

    public async Task<City> GetCityAsync(City city)
    {
        return await _dataContextMsSql.Cities.FindAsync(city.Id);
    }


    public Task<IIncludableQueryable<Country, City>>
        GetCityWithCountryAsync(int id)
    {
        return Task.FromResult(_dataContextMsSql.Countries
            .Include(c => c.Cities
                .FirstOrDefault(ci => ci.Id == id)));
    }

    public Task<IIncludableQueryable<Country, City>>
        GetCityWithCountryAsync(City city)
    {
        return Task.FromResult(_dataContextMsSql.Countries
            .Include(c => c.Cities
                .FirstOrDefault(ci => ci.Id == city.Id)));
    }

    public Task<IIncludableQueryable<Country, City>>
        GetCityWithCountryAsync(Country country)
    {
        return Task.FromResult(_dataContextMsSql.Countries
            .Include(c => c.Cities
                .FirstOrDefault(ci => ci.Id == country.Id)));
    }


    public async Task AddCityAsync(CityViewModel model)
    {
        var country =
            await GetCountryWithCitiesAsync(model.CountryId);

        if (country == null) return;

        var city = new City {Name = model.Name};

        _dataContextMsSql.Cities.Add(city);

        await _dataContextMsSql.SaveChangesAsync();
    }

    public async Task<int> UpdateCityAsync(City city)
    {
        var country =
            await _dataContextMsSql.Countries
                .Where(c => c.Cities.Any(ci => ci.Id == city.Id))
                .FirstOrDefaultAsync();

        if (country == null) return 0;

        _dataContextMsSql.Cities.Update(city);

        await _dataContextMsSql.SaveChangesAsync();

        return country.Id;
    }

    public async Task<int> DeleteCityAsync(City city)
    {
        var country =
            await _dataContextMsSql.Countries
                .Where(c => c.Cities.Any(ci => ci.Id == city.Id))
                .FirstOrDefaultAsync();

        if (country == null) return 0;

        _dataContextMsSql.Cities.Remove(city);

        await _dataContextMsSql.SaveChangesAsync();

        return country.Id;
    }
}