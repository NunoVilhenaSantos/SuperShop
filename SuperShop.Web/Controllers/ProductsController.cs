﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.Repositories;
using SuperShop.Web.Helpers;
using SuperShop.Web.Models;

namespace SuperShop.Web.Controllers;

// [Authorize]
public class ProductsController : Controller
{
    private readonly IConverterHelper _converterHelper;
    private readonly IImageHelper _imageHelper;
    private readonly IProductsRepository _productsRepository;
    private readonly IStorageHelper _storageHelper;
    private readonly IUserHelper _userHelper;

    // private readonly IRepository _repository;
    // private readonly DataContextMSSQL _context;


    public ProductsController(
        IUserHelper userHelper,
        IImageHelper imageHelper,
        IStorageHelper storageHelper,
        IConverterHelper converterHelper,
        IProductsRepository productsRepository
    )
    {
        _userHelper = userHelper;
        _imageHelper = imageHelper;
        _storageHelper = storageHelper;
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
        // if (id == null) return NotFound();
        if (id == null) return new NotFoundViewResult("ProductNotFound");

        // var product =
        //     await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
        // var product = _repository.GetProduct(id.Value);
        var product = await _productsRepository.GetByIdAsync(id.Value);

        // if (product == null) return NotFound();
        return product == null
            ? new NotFoundViewResult("ProductNotFound")
            : View(product);
    }


    [Authorize(Roles = "Admin")]
    // GET: Products/Create
    public IActionResult Create()
    {
        return View();
    }


    // POST: Products/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        ProductViewModel productViewModel)
    {
        if (!ModelState.IsValid) return View(productViewModel);

        var filePath = productViewModel.ImageUrl;
        var fileStorageId = productViewModel.ImageId;

        if (productViewModel.ImageFile is {Length: > 0})
        {
            filePath = await _imageHelper.UploadImageAsync(
                productViewModel.ImageFile, "products");


            // fileStorageId = await _storageHelper.UploadStorageAsync(
            //     productViewModel.ImageFile, "products");


            fileStorageId = await _storageHelper.UploadFileAsyncToGcp(
                productViewModel.ImageFile,
                "products");

            fileStorageId = await _storageHelper.UploadFileAsyncToGcp(
                productViewModel.ImageFile.FileName,
                "products");
        }

        // TODO: Pending to improve
        var product = _converterHelper.ToProduct(
            productViewModel, filePath, fileStorageId, false);


        // product.User = await _userHelper.GetUserByEmailAsync(
        //     "nunovilhenasantos@msn.com");
        product.User =
            await _userHelper.GetUserByEmailAsync(User.Identity.Name);

        await _productsRepository.CreateAsync(product);

        return RedirectToAction(nameof(Index));
    }


    [Authorize]
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
        if (id != productViewModel.Id)
            return new NotFoundViewResult("ProductNotFound");


        if (!ModelState.IsValid) return View(productViewModel);


        try
        {
            var filePath = productViewModel.ImageUrl;
            var fileStorageId = productViewModel.ImageId;

            if (productViewModel.ImageFile is {Length: > 0})
            {
                filePath = await _imageHelper.UploadImageAsync(
                    productViewModel.ImageFile, "products");

                // fileStorageId = await _storageHelper.UploadStorageAsync(
                //     productViewModel.ImageFile, "products");

                fileStorageId = await _storageHelper.UploadFileAsyncToGcp(
                    productViewModel.ImageFile, "products");
            }


            var product = _converterHelper.ToProduct(
                productViewModel, filePath, fileStorageId, false);


            // product.User =
            //     await _userHelper.GetUserByEmailAsync(
            //         "nunovilhenasantos@msn.com");
            product.User =
                await _userHelper.GetUserByEmailAsync(User.Identity.Name);


            await _productsRepository.UpdateAsync(product);
            // await _repository.SaveAllAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _productsRepository.ExistAsync(productViewModel.Id))
                return new NotFoundViewResult("ProductNotFound");
            // throw;
        }

        return RedirectToAction(nameof(Index));
    }


    [Authorize]
    // GET: Products/Delete/5
    //public async Task<IActionResult> Delete(int? id)
    public async Task<IActionResult> Delete(
        int? id, bool showErrorModal = false)
    {
        if (id == null) return new NotFoundViewResult("ProductNotFound");

        // var product = _repository.GetProduct(id.Value);
        var product = await _productsRepository.GetByIdAsync(id.Value);

        return product == null
            ? new NotFoundViewResult("ProductNotFound")
            : View(product);
    }


    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // var product = _repository.GetProduct(id);
        var product = await _productsRepository.GetByIdAsync(id);


        // product.User =
        //     await _userHelper.GetUserByEmailAsync(
        //         "nunovilhenasantos@msn.com");
        product.User =
            await _userHelper.GetUserByEmailAsync(User.Identity.Name);

        try
        {
            // _repository.DeleteProduct(product);
            await _productsRepository.DeleteAsync(product);
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine(ex.Message);

            if (ex.InnerException == null ||
                !ex.InnerException.Message.Contains("DELETE"))
                return View("Error");


            TempData["saveChangesError"] = true;

            TempData["ErrorTitle"] =
                "Provavelmente está a ser usado!!";


            TempData["ErrorMessage"] =
                $"{product.Name} não pode ser apagado visto " +
                $"existirem encomendas que o usam.</br></br>" +
                $"Experimente primeiro apagar todas as encomendas " +
                $"que o estão a usar, e torne novamente a apagá-lo";


            TempData["DbUpdateException"] = ex.Message;
            TempData["DbUpdateInnerException"] = ex.InnerException;
            TempData["DbUpdateInnerExceptionMessage"] =
                ex.InnerException.Message;


            return RedirectToAction(
                nameof(Delete),
                new {id = product.Id, showErrorModal = true});

            // return RedirectToAction(nameof(Delete));


            // return View("Error");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            if (ex.InnerException == null ||
                !ex.InnerException.Message.Contains("DELETE"))
                return View("Error");

            TempData["ErrorTitle"] =
                $"{product.Name} provavelmente está a ser usado!!";

            TempData["ErrorMessage"] =
                $"{product.Name} não pode ser apagado visto " +
                $"haverem encomendas que o usam.</br></br>" +
                $"Experimente primeiro apagar todas as encomendas " +
                $"que o estão a usar, e torne novamente a apagá-lo";

            return View("Error");
        }


        // await _repository.SaveAllAsync();
        return RedirectToAction(nameof(Index));
    }


    public IActionResult ProductNotFound()
    {
        return View();
    }


    //private bool ProductExists(int id)
    //{
    //    return _context.Products.Any(e => e.Id == id);
    //}
}