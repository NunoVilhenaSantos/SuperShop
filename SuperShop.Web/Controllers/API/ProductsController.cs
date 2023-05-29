using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Web.Data;
using SuperShop.Web.Helpers;

namespace SuperShop.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IProductsRepository _productsRepository;

        public ProductsController(
            IProductsRepository productsRepository, IUserHelper userHelper)
        {
            _userHelper = userHelper;
            _productsRepository = productsRepository;
        }


        // GET: api/Products
        [HttpGet]
        public IActionResult Get()
        {
            var products = _productsRepository.GetAll();

            // foreach (var p in products)
            // {
            //     p.User = _userHelper.GetUserByIdAsync(p.User.Id).Result;
            // }
            // return Ok(products);

            return Ok(_productsRepository.GetAll());
        }

        // GET: api/Products/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Products
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}