using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data;
using SuperShop.Web.Data.Entity;
using SuperShop.Web.Helpers;
using SuperShop.Web.Models;

namespace SuperShop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IUserHelper _userHelper;

        // private readonly IRepository _repository;
        // private readonly DataContext _context;


        public ProductsController(
            IProductsRepository productsRepository, IUserHelper userHelper)
        {
            _productsRepository = productsRepository;
            _userHelper = userHelper;

            // _repository = repository;
        }


        // GET: Products
        // public async Task<IActionResult> Index()
        public IActionResult Index()
        {
            // return View(await _context.Products.ToListAsync());
            // return View(_repository.GetProducts());
            return View(
                _productsRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Products/Details/5
        //public async Task<IActionResult> Details(int? id)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // var product =
            //     await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            // var product = _repository.GetProduct(id.Value);
            var product = await _productsRepository.GetByIdAsync(id.Value);

            if (product == null) return NotFound();

            return View(product);
        }


        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Products/Create
        // To protect from overposting attacks,
        // enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ProductViewModal productViewModal)
        {
            if (!ModelState.IsValid) return View(productViewModal);

            var filePath = productViewModal.ImageUrl;

            if (productViewModal.ImageFile is {Length: > 0})
                await ToImages(productViewModal, filePath);

            var product = ToProduct(productViewModal, filePath);

            // TODO: Pending to improve
            product.User =
                await _userHelper.GetUserByEmailAsync(
                    "nunovilhenasantos@msn.com");
            // product.User = 
            //     await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            await _productsRepository.CreateAsync(product);

            return RedirectToAction(nameof(Index));
        }

        private static Product ToProduct(
            ProductViewModal productViewModal, string filePath)
        {
            return new Product
            {
                Id = productViewModal.Id,
                ImageUrl = filePath,
                IsAvailable = productViewModal.IsAvailable,
                LastPurchase = productViewModal.LastPurchase,
                LastSale = productViewModal.LastSale,
                Name = productViewModal.Name,
                Price = productViewModal.Price,
                Stock = productViewModal.Stock,
                User = productViewModal.User
            };
        }


        // GET: Products/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        public async Task<IActionResult> Edit(int? id)
        {
            // if (id == null) return NotFound();
            if (id == null) return RedirectToAction(nameof(Index));

            // var product = await _context.Products.FindAsync(id);
            // var product = _repository.GetProduct(id.Value);
            var product = await _productsRepository.GetByIdAsync(id.Value);

            if (product == null) return RedirectToAction(nameof(Index));

            var productViewModal = ToProductViewModal(product);

            return View(productViewModal);
        }

        private object ToProductViewModal(Product product)
        {
            return new ProductViewModal
            {
                Id = product.Id,
                ImageUrl = product.ImageUrl,
                // ImageFile = product.ImageUrl,
                IsAvailable = product.IsAvailable,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = product.User
            };
        }


        // POST: Products/Edit/5
        // To protect from overposting attacks,
        // enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, ProductViewModal productViewModal)
        {
            if (id != productViewModal.Id) return NotFound();

            if (!ModelState.IsValid) return View(productViewModal);

            try
            {
                var filePath = productViewModal.ImageUrl;

                if (productViewModal.ImageFile is {Length: > 0})
                    await ToImages(productViewModal, filePath);

                var product = ToProduct(productViewModal, filePath);

                // TODO: Pending to improve
                product.User =
                    await _userHelper.GetUserByEmailAsync(
                        "nunovilhenasantos@msn.com");
                // product.User = 
                //     await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                await _productsRepository.UpdateAsync(productViewModal);
                // await _repository.SaveAllAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _productsRepository.ExistAsync(productViewModal.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }


        private static async Task ToImages(
            ProductViewModal productViewModal,
            string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            var guid = Guid.NewGuid().ToString();
            var fileName = guid + ".jpg";

            filePath = Directory.GetCurrentDirectory() +
                       "\\wwwroot\\images\\Products\\" +
                       fileName;

            await using var stream =
                new FileStream(
                    filePath, FileMode.Create, FileAccess.ReadWrite);

            await productViewModal.ImageFile.CopyToAsync(stream);

            // path = await _imageHelper.UploadImageAsync(
            //     productViewModal.ImageFile);

            filePath = "~/images/Products/" +
                       fileName;

            productViewModal.ImageUrl = filePath;
        }


        // GET: Products/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            // var product = _repository.GetProduct(id.Value);
            var product = await _productsRepository.GetByIdAsync(id.Value);

            if (product == null) return NotFound();

            return View(product);
        }


        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // var product = _repository.GetProduct(id);
            var product = await _productsRepository.GetByIdAsync(id);

            // TODO: Pending to improve
            product.User =
                await _userHelper.GetUserByEmailAsync(
                    "nunovilhenasantos@msn.com");

            // product.User = 
            //     await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            // _repository.DeleteProduct(product);
            await _productsRepository.DeleteAsync(product);

            // await _repository.SaveAllAsync();
            return RedirectToAction(nameof(Index));
        }


        //private bool ProductExists(int id)
        //{
        //    return _context.Products.Any(e => e.Id == id);
        //}
    }
}