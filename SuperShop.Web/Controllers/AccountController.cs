using Microsoft.AspNetCore.Mvc;
using SuperShop.Web.Helpers;

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


    // public IActionResult Index()
    // {
    //     return View();
    // }
}