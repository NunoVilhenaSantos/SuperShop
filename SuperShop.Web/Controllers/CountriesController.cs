using Microsoft.AspNetCore.Mvc;
using SuperShop.Web.Data.Repositories;

namespace SuperShop.Web.Controllers;

public class CountriesController : Controller
{
    private readonly ICountryRepository _countryRepository;


    public CountriesController(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }


    // GET
    public IActionResult Index()
    {
        return View(_countryRepository.GetCountriesWithCities());
    }
}