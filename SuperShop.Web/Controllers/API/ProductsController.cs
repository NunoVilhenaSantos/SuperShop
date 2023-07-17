using Azure.Core.Pipeline;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using SuperShop.Web.Data.Repositories;
using SuperShop.Web.Helpers;


namespace SuperShop.Web.Controllers.API;

[Route("api/[controller]")]
[ApiController]
// [Authorize(Policy = "Bearer")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
// [Authorize(Roles = "Admin")]
public class ProductsController : Controller
{
    private readonly IProductsRepository _productsRepository;
    private readonly IUserHelper _userHelper;

    public ProductsController(
        IUserHelper userHelper, IProductsRepository productsRepository)
    {
        _userHelper = userHelper;
        _productsRepository = productsRepository;
    }


    // GET: api/Products
    [HttpGet]
    public IActionResult GetProducts()
    {
        // var products = _productsRepository.GetAll();

        // foreach (var p in products)
        // {
        //     p.User = _userHelper.GetUserByIdAsync(p.User.Id).Result;
        // }

        // return Ok(products);

        return Ok(_productsRepository.GetAllWithUsers());
    }

    // GET: api/Products/5
    [HttpGet("id", Name = "Get")]
    public string GetProduct(int id)
    {
        return "value";
    }

    // POST: api/Products
    [HttpPost]
    public void PostProduct([FromBody] string value, int id)
    {
    }

    // PUT: api/Products/5
    [HttpPut("id")]
    public void PutProduct(int id, [FromBody] string value)
    {
    }

    // DELETE: api/Products/5
    [HttpDelete("id")]
    public void DeleteProduct(int id)
    {
    }
}