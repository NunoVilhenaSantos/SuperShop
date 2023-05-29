using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data;
using SuperShop.Web.Data.Entity;
using SuperShop.Web.Helpers;

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
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid) return View(product);

            // _context.Add(product);
            // await _context.SaveChangesAsync();

            // _repository.AddProduct(product);
            // await _repository.SaveAllAsync();
            product.User = 
                await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

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

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks,
        // enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id,
        //    [Bind("Id,Name,Price,ImageUrl," +
        //          "LastPurchase,LastSale,IsAvailable,Stock")]
        //    Product product)
        //{
        //    if (id != product.Id) return NotFound();

        //    if (!ModelState.IsValid) return View(product);

        //    try
        //    {
        //        _context.Update(product);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductExists(product.Id))
        //            return NotFound();
        //        throw;
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        // POST: Products/Edit/5
        // To protect from overposting attacks,
        // enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id) return NotFound();

            if (!ModelState.IsValid) return View(product);

            try
            {
                // _repository.UpdateProduct(product);
                // TODO: Pending to improve
                product.User =
                    await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                await _productsRepository.UpdateAsync(product);
                // await _repository.SaveAllAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _productsRepository.ExistAsync(product.Id))
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


        // POST: Products/Delete/5
        //[HttpPost]
        //[ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);
        //    _context.Products.Remove(product);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // var product = _repository.GetProduct(id);
            var product = await _productsRepository.GetByIdAsync(id);

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