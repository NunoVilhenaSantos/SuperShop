﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuperShop.Web.Models;

namespace SuperShop.Web.Controllers;

public class ErrorsController : Controller
{
    private readonly ILogger<ErrorsController> _logger;


    public ErrorsController(ILogger<ErrorsController> logger)
    {
        _logger = logger;
    }


    [ResponseCache(
        Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }


    [Route("error/403")]
    public IActionResult Error403()
    {
        return View();
    }


    [Route("error/404")]
    public IActionResult Error404()
    {
        return View();
    }
}