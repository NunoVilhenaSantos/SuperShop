﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SuperShop.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;


    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }


    public IActionResult Index()
    {
        return View();
    }

    public ActionResult About()
    {
        var currentTime = DateTime.Now.ToLongTimeString();
        ViewBag.Message = "The current time is " + currentTime;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }


    // [ResponseCache(
    //     Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel
    //     {
    //         RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
    //     });
    // }


    // [Route("error/404")]
    // public IActionResult Error404()
    // {
    //     return View();
    // }
}