using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Web.Data.Repositories;
using SuperShop.Web.Models;

namespace SuperShop.Web.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductsRepository _productsRepository;


    public OrdersController(
        IOrderRepository orderRepository,
        IProductsRepository productsRepository
    )
    {
        _orderRepository = orderRepository;
        _productsRepository = productsRepository;
    }


    //HttpGet
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model =
            await _orderRepository
                .GetOrdersAsync(User.Identity.Name);

        return View(model);
    }


    //HttpGet
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model =
            await _orderRepository
                .GetDetailsTempAsync(User.Identity.Name);

        return View(model);
    }


    //HttpGet
    [HttpGet]
    public IActionResult AddProduct()
    {
        var model = new AddItemViewModel
        {
            Products = _productsRepository.GetComboProducts()
        };

        return View(model);
    }


    //HttpPost
    [HttpPost]
    public async Task<IActionResult> AddProduct(AddItemViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await _orderRepository.AddItemToOrderAsync(model, User.Identity.Name);

        return RedirectToAction(nameof(Create));
    }


    //HttpGet
    [HttpGet]
    public async Task<IActionResult> Increase(int? id)
    {
        if (id == null) return NotFound();

        await _orderRepository.ModifyOrderDetailTempQuantityAsync(
            id.Value, 1);

        return RedirectToAction(nameof(Create));
    }


    //HttpGet
    [HttpGet]
    public async Task<IActionResult> Decrease(int? id)
    {
        if (id == null) return NotFound();

        await _orderRepository.ModifyOrderDetailTempQuantityAsync(
            id.Value, -1);

        return RedirectToAction(nameof(Create));
    }


    //HttpGet
    [HttpGet]
    public async Task<IActionResult> DeleteItem(int? id)
    {
        if (id == null) return NotFound();

        await _orderRepository.DeleteDetailTempAsync(id.Value);

        return RedirectToAction(nameof(Create));
    }


    //HttpGet
    [HttpGet]
    public async Task<IActionResult> ConfirmOrder()
    {
        var response =
            await _orderRepository.ConfirmOrderAsync(User.Identity.Name);

        return RedirectToAction(response ? nameof(Index) : nameof(Create));
    }
}