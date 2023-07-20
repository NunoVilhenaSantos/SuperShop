using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SuperShop.Web.Data.Entities;
using SuperShop.Web.Data.Repositories;
using SuperShop.Web.Helpers;
using SuperShop.Web.Models;

namespace SuperShop.Web.Controllers;

public class AccountController : Controller
{
    private readonly ICountryRepository _countryRepository;
    private readonly IConfiguration _configuration;
    private readonly IUserHelper _userHelper;


    public AccountController(
        IUserHelper userHelper,
        IConfiguration configuration,
        ICountryRepository countryRepository)
    {
        _userHelper = userHelper;
        _configuration = configuration;
        _countryRepository = countryRepository;
    }


    // Aqui o utilizador é reencaminhado para a view de Login
    // caso não esteja autenticado
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity is {IsAuthenticated: true})
            return RedirectToAction("Index", "Home");

        return View();
    }


    // Aqui é que se valida as informações do usuário
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userHelper.LoginAsync(model);

            if (result.Succeeded)
                //
                // Caso tente acessar outra view diferente do
                // Login sou direcionado para a view Login,
                //
                // mas após sou direcionado para a view
                // que tentei acessar em primeiro lugar.
                //
                // Exemplo: ProductsController [Authorize]
                //
                return Request.Query.Keys.Contains("ReturnUrl")
                    ? Redirect(Request.Query["ReturnUrl"].First())
                    : RedirectToAction("Index", "Home");

            ModelState.AddModelError(
                string.Empty, "Failed to login!");
            return View(model);
        }

        ModelState.AddModelError(
            string.Empty, "Failed to login!");
        return View(model);
    }


    // Aqui o utilizador faz o logout da sua conta
    // Aqui o utilizador é reencaminhado para a view Index do controlador Home
    [HttpGet]
    public async Task<IActionResult> LogOut()
    {
        await _userHelper.LogOutAsync();
        return RedirectToAction("Index", "Home");
    }


    // aqui vai para a view RegisterNewUserViewModel
    [HttpGet]
    public IActionResult Register()
    {
        var model = new RegisterNewUserViewModel
        {
            Countries = _countryRepository.GetComboCountries(),
            Cities = _countryRepository.GetComboCities(0)
        };


        return View(model);
    }


    // Aqui é que se valida as informações do utilizador
    [HttpPost]
    public async Task<IActionResult> Register(RegisterNewUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.Username);

            if (user == null)
            {
                var city = await _countryRepository.GetCityAsync(model.CityId);

                user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Username,
                    UserName = model.Username,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    City = city,
                    // CityId = model.CityId,
                    // Country = model.CountryId,
                    CountryId = model.CountryId
                };


                var result =
                    await _userHelper.AddUserAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var loginViewModel = new LoginViewModel
                    {
                        Password = model.Password,
                        RememberMe = false,
                        Username = model.Username
                    };


                    var result2 = await _userHelper.LoginAsync(loginViewModel);
                    if (result2.Succeeded)
                        return RedirectToAction("Index", "Home");

                    ModelState.AddModelError(string.Empty,
                        "The User couldn't be logged.");
                    return View(model);
                }

                ModelState.AddModelError(string.Empty,
                    "The User couldn't be created.");
                return View(model);
            }

            ModelState.AddModelError(
                string.Empty, "User already exists.");
            return View(model);
            // return RedirectToAction("Login", "Account");
        }

        ModelState.AddModelError(
            string.Empty, "Tem de preencher os campos!");


        return View(model);
    }


    // Aqui o utilizador faz as alterações aos seus dados da sua conta
    [HttpGet]
    public async Task<IActionResult> ChangeUser()
    {
        var user = await _userHelper.GetUserByEmailAsync(User.Identity?.Name);

        if (user == null) return View();

        var model = new ChangeUserViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            Username = user.UserName,
            Cities = _countryRepository.GetComboCities(user.CountryId),
            CityId = user.City?.Id ?? 0,
            Countries = _countryRepository.GetComboCountries(),
            CountryId = user.CountryId
            // Email = user.Email,
        };

        return View(model);
    }


    // Aqui é que se alteram as informações do utilizador
    [HttpPost]
    public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user =
                await _userHelper.GetUserByEmailAsync(User.Identity?.Name);

            if (user == null) return View();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.Username;
            // user.City = _countryRepository.GetCityAsync(model.CityId).Result;
            // user.CountryId =
            //     _countryRepository.GetCountryAsync(user.City).Result.Id;
            user.City = await _countryRepository.GetCityAsync(model.CityId);
            user.CountryId = model.CountryId;
            // user.Email = model.Username;

            var response = await _userHelper.UpdateUserAsync(user);

            if (response.Succeeded)
            {
                await _userHelper.AddUserToRoleAsync(user, "Customer");
                ViewBag.UserMessage = "User updated!";
            }
            else
            {
                var errorMessage =
                    response.Errors.FirstOrDefault()?.Description;
                if (errorMessage != null)
                    ModelState.AddModelError(string.Empty,
                        errorMessage);
            }


            return View(model);
        }

        ModelState.AddModelError(
            string.Empty, "Failed to login!");

        return View();
    }


    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> ChangePassword(
        ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userHelper.GetUserByEmailAsync(User.Identity?.Name);

        if (user == null) return View();

        var response = await _userHelper.ChangePasswordAsync(
            user, model.OldPassword, model.NewPassword);

        if (response.Succeeded)
        {
            ViewBag.UserMessage = "Password changed!";
            return RedirectToAction("ChangeUser");
        }

        var errorMessage =
            response.Errors.FirstOrDefault()?.Description;
        if (errorMessage != null)
            ModelState.AddModelError(
                string.Empty, errorMessage);
        ModelState.AddModelError(
            string.Empty, "User not found.");


        return View(model);

        // ModelState.AddModelError(
        //     string.Empty, "Failed to login!");
    }


    // Aqui o utilizador obtem a lista de cidades de um determinado pais
    [HttpPost]
    // [Route("api/Account/GetCitiesAsync")]
    [Route("Account/GetCitiesAsync")]
    public async Task<JsonResult> GetCitiesAsync(int countryId)
    {
        var country =
            await _countryRepository.GetCountryWithCitiesAsync(countryId);

        return Json(country.Cities.OrderBy(c => c.Name));
    }


    // Aqui o utilizador obtem a lista de paises
    [HttpPost]
    // [Route("api/Account/GetCitiesAsync")]
    [Route("Account/GetCountriesAsync")]
    public Task<JsonResult> GetCountriesAsync()
    {
        var country =
            _countryRepository.GetCountriesWithCitiesEnumerable();

        return Task.FromResult(Json(country.OrderBy(c => c.Name)));
    }


    // https://localhost:5001/Account/NotAuthorized
    [HttpGet]
    public IActionResult NotAuthorized()
    {
        return View();
    }


    // https://localhost:5001/Account/AccessDenied
    // [HttpGet]
    // public IActionResult AccessDenied()
    // {
    //     return View();
    // }


    // https://localhost:5001/Account/Error
    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }


    // https://localhost:5001/Account/CreateToken
    // [Route("Account/CreateToken")]
    [HttpPost]
    public async Task<IActionResult> CreateToken(
        [FromBody] LoginViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest();


        var user = await _userHelper.GetUserByEmailAsync(model.Username);

        if (user == null) return BadRequest();


        var result = await _userHelper.ValidatePasswordAsync(
            user, model.Password);

        if (!result.Succeeded) return BadRequest();


        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

        var credentials = new SigningCredentials(key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Tokens:Issuer"],
            _configuration["Tokens:Audience"],
            claims,
            expires: DateTime.UtcNow.AddDays(15),
            signingCredentials: credentials);

        var results = new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        };

        return Created(string.Empty, results);
    }


    // Adicione um método para calcular o tempo restante de expiração do token
    private TimeSpan GetTimeRemaining(DateTime expirationDate)
    {
        return expirationDate - DateTime.UtcNow;
    }

}