using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;
using SuperShop.Web.Data.Entities;
using SuperShop.Web.Models;

namespace SuperShop.Web.Data.Repositories;

public interface ICountryRepository : IGenericRepository<Country>
{
    IQueryable<Country> GetCountriesWithCities();

    IEnumerable<Country> GetCountriesWithCitiesEnumerable();

    IEnumerable<Country> GetCountriesWithCitiesEnumerableNoTracking();


    IEnumerable<SelectListItem> GetComboCountries();

    IEnumerable<SelectListItem> GetComboCities(int countryId);


    Task<Country> GetCountryWithCitiesAsync(int id);

    Task<Country> GetCountryWithCitiesAsync(Country country);

    Task<Country> GetCountryWithCitiesAsync(City city);


    Task<City> GetCityAsync(int id);
    Task<City> GetCityAsync(City city);

    Task<IIncludableQueryable<Country, City>> GetCityWithCountryAsync(int id);

    Task<IIncludableQueryable<Country, City>> GetCityWithCountryAsync(
        City city);

    Task<IIncludableQueryable<Country, City>> GetCityWithCountryAsync(
        Country country);


    Task AddCityAsync(CityViewModel model);

    Task<int> UpdateCityAsync(City city);

    Task<int> DeleteCityAsync(City city);
}