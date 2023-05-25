﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data;
using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IRepository _repository;
        //private readonly DataContext _context;


        public ProductsController(IRepository repository)
        {
            _repository = repository;
        }


        // GET: Products
        // public async Task<IActionResult> Index()
        public IActionResult Index()
        {
            //return View(await _context.Products.ToListAsync());
            return View( _repository.GetProducts());
        }

        // GET: Products/Details/5
        //public async Task<IActionResult> Details(int? id)
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            // var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            var product = _repository.GetProduct(id.Value);

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

            //_context.Add(product);
            //await _context.SaveChangesAsync();

            _repository.AddProduct(product);
            await _repository.SaveAllAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Products/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        public IActionResult Edit(int? id)
        {
            // if (id == null) return NotFound();
            if (id == null) return RedirectToAction(nameof(Index));

            //var product = await _context.Products.FindAsync(id);
            var product = _repository.GetProduct(id.Value);
            
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
                _repository.UpdateProduct(product);
                await _repository.SaveAllAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.ProductExists(product.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }



        // GET: Products/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = _repository.GetProduct(id.Value);

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
            var product = _repository.GetProduct(id);
            
            _repository.DeleteProduct(product);

            await _repository.SaveAllAsync();

            return RedirectToAction(nameof(Index));
        }


        //private bool ProductExists(int id)
        //{
        //    return _context.Products.Any(e => e.Id == id);
        //}
    }
}