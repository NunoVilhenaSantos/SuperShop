using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Web.Data.Entities;
using SuperShop.Web.Data.Repositories;
using SuperShop.Web.Models;

namespace SuperShop.Web.Controllers;

[Authorize(Roles = "Admin")]
public class CountriesController : Controller
{
    private readonly ICountryRepository _countryRepository;


    public CountriesController(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }


    // ------------------------------ --------- ----------------------------- //
    // ------------------------------ Countries ----------------------------- //
    // ------------------------------ --------- ----------------------------- //


    // GET: Countries
    [HttpGet]
    public IActionResult Index()
    {
        return View(_countryRepository.GetCountriesWithCities());
    }


    // GET: Countries/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var country =
            await _countryRepository.GetCountryWithCitiesAsync(id.Value);

        if (country == null) return NotFound();

        return View(country);
    }


    // GET: Countries/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    // POST: Countries/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Country country)
    {
        if (!ModelState.IsValid) return View(country);

        await _countryRepository.CreateAsync(country);

        return RedirectToAction(nameof(Index));
    }


    // GET: Countries/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var country = await _countryRepository.GetByIdAsync(id.Value);

        if (country == null) return NotFound();

        return View(country);
    }


    // POST: Countries/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Country country)
    {
        if (!ModelState.IsValid) return View(country);

        await _countryRepository.UpdateAsync(country);

        return RedirectToAction(nameof(Index));
    }


    // GET: Countries/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var country = await _countryRepository.GetByIdAsync(id.Value);

        if (country == null) return NotFound();

        await _countryRepository.DeleteAsync(country);

        return RedirectToAction(nameof(Index));
    }


    // ------------------------------- ------ ------------------------------- //
    // ------------------------------- Cities ------------------------------- //
    // ------------------------------- ------ ------------------------------- //


    // GET: Countries/AddCity/5
    [HttpGet]
    public async Task<IActionResult> AddCity(
        int? id, int countryId, string countryName, int method)
    {
        if (id == null) return NotFound();

        var country = await _countryRepository.GetByIdAsync(id.Value);

        if (country == null) return NotFound();


        CityViewModel model;
        switch (method)
        {
            case 1:
                // Passe as informações do país para a vista
                model = new CityViewModel {CountryId = country.Id};
                break;
            case 2:
                // Passe as informações do país para a vista
                model = new CityViewModel
                {
                    CountryId = country.Id,
                    CountryName = country.Name
                };
                break;
            default:
                // algo deu errado
                return NotFound();
        }


        return View(model);
    }


    // POST: Countries/AddCity
    [HttpPost]
    public async Task<IActionResult> AddCity(CityViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await _countryRepository.AddCityAsync(model);

        return RedirectToAction(
            nameof(Details), new {id = model.CountryId});
    }


    // GET: Countries/DeleteCity/5
    [HttpGet]
    public async Task<IActionResult> DeleteCity(int? id)
    {
        if (id == null) return NotFound();

        var city = await _countryRepository.GetCityAsync(id.Value);

        if (city == null) return NotFound();

        var countryId = await _countryRepository.DeleteCityAsync(city);

        return RedirectToAction(nameof(Details), new {id = countryId});
    }


    // GET: Countries/EditCity/5
    [HttpGet]
    public async Task<IActionResult> EditCity(
        int? id, int countryId, string countryName)
    {
        if (id == null) return NotFound();

        var city = await _countryRepository.GetCityAsync(id.Value);

        if (city == null) return NotFound();

        ViewData["CountryId"] = countryId;
        ViewData["CountryName"] = countryName;

        return View(city);
    }


    // POST: Countries/EditCity/5
    [HttpPost]
    public async Task<IActionResult> EditCity(City city)
    {
        if (!ModelState.IsValid) return View(city);

        var countryId = await _countryRepository.UpdateCityAsync(city);

        if (countryId != 0)
            return RedirectToAction(
                nameof(Details), new {id = countryId});

        return View(city);
    }
}