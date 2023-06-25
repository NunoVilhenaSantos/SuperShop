using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Web.Data.Entity;
using SuperShop.Web.Helpers;
using SuperShop.Web.Models;

namespace SuperShop.Web.Controllers;

public class AccountController : Controller
{
    private readonly IUserHelper _userHelper;


    public AccountController(IUserHelper userHelper)
    {
        _userHelper = userHelper;
    }


    // Aqui o utilizador é reencaminhado para a view de Login
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


    public async Task<IActionResult> LogOut()
    {
        await _userHelper.LogOutAsync();
        return RedirectToAction("Index", "Home");
    }


    // aqui vai para a view RegisterNewUserViewModel
    public IActionResult Register()
    {
        return View();
    }


    // Aqui é que se valida as informações do usuário
    [HttpPost]
    public async Task<IActionResult> Register(RegisterNewUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.Username);

            if (user == null)
            {
                user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Username,
                    UserName = model.Username
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

            ModelState.AddModelError(string.Empty, "User already exists.");
            return View(model);
            // return RedirectToAction("Login", "Account");
        }

        ModelState.AddModelError(
            string.Empty, "Tem de preencher os campos!");
        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> ChangeUser()
    {
        var user = await _userHelper.GetUserByEmailAsync(User.Identity?.Name);

        if (user == null) return View();

        var model = new ChangeUserViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName
            // Address = user.Result.Address,
            // PhoneNumber = user.Result.PhoneNumber,
            // UserName = user.Result.UserName,
            // Email = user.Result.Email,
        };

        return View(model);
    }


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
            // user.Result.Address,
            // user.Result.PhoneNumber,
            // user.Result.UserName,
            // user.Result.Email,

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


    public IActionResult NotAuthorized()
    {
        return View();
    }
}