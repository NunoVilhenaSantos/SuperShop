using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Web.Data.Repositories;


namespace SuperShop.Web.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly IOrderRepository _orderRepository;


    public OrdersController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }


    public async Task<IActionResult> Index()
    {
        var model = await _orderRepository
            .GetOrdersFromUsersAsync(User.Identity.Name);

        return View();
    }
}