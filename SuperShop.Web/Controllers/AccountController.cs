using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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


    public IActionResult Login()
    {
        if (User.Identity is {IsAuthenticated: true})
            return RedirectToAction("Index", "Home");

        return View();
    }


    // Aqui é que de fato valida as informações do usuário
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userHelper.LoginAsync(model);

            if (result.Succeeded)
            {
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
            }
            else
            {
                ModelState.AddModelError(
                    string.Empty, "Failed to login!");
                return View(model);
            }
        }
        else
        {
            ModelState.AddModelError(
                string.Empty, "Failed to login!");
            return View(model);
        }
    }

    // public IActionResult Index()
    // {
    //     return View();
    // }

    public async Task<IActionResult> LogOut()
    {
        await _userHelper.LogOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult ChangeUser()
    {
        throw new System.NotImplementedException();
    }

    public IActionResult Register()
    {
        throw new System.NotImplementedException();
    }
}