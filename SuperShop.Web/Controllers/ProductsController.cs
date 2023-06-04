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
        #region Attributes

        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IProductsRepository _productsRepository;

        // private readonly IRepository _repository;
        // private readonly DataContext _context;

        #endregion

        public ProductsController(IProductsRepository productsRepository,
            IUserHelper userHelper, IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _productsRepository = productsRepository;

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
            ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid) return View(productViewModel);

            var filePath = productViewModel.ImageUrl;

            if (productViewModel.ImageFile is {Length: > 0})
                filePath = await _imageHelper.UploadImageAsync(
                    productViewModel.ImageFile, "products");

            var product = _converterHelper.ToProduct(
                productViewModel, filePath, true);

            // TODO: Pending to improve
            product.User =
                await _userHelper.GetUserByEmailAsync(
                    "nunovilhenasantos@msn.com");
            // product.User = 
            //     await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            await _productsRepository.CreateAsync(product);

            return RedirectToAction(nameof(Index));
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

            var productViewModal =
                _converterHelper.ToProductViewModel(product);

            return View(productViewModal);
        }


        // POST: Products/Edit/5
        // To protect from overposting attacks,
        // enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(productViewModel);

            try
            {
                var filePath = productViewModel.ImageUrl;

                if (productViewModel.ImageFile is {Length: > 0})
                    filePath = await _imageHelper.UploadImageAsync(
                        productViewModel.ImageFile, "products");

                var product = _converterHelper.ToProduct(
                    productViewModel, filePath, false);

                // TODO: Pending to improve
                product.User =
                    await _userHelper.GetUserByEmailAsync(
                        "nunovilhenasantos@msn.com");
                // product.User = 
                //     await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                await _productsRepository.UpdateAsync(product);
                // await _repository.SaveAllAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _productsRepository.ExistAsync(productViewModel.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
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