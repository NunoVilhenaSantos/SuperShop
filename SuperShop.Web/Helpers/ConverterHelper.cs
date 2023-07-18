using System;
using SuperShop.Web.Data.Entities;
using SuperShop.Web.Models;

namespace SuperShop.Web.Helpers;

public class ConverterHelper : IConverterHelper
{
    public Product ToProduct(ProductViewModel productViewModel,
        string filePath, Guid fileStorage, bool isNew)
    {
        return new Product
        {
            Id = isNew
                ? 0
                : productViewModel.Id,
            Name = productViewModel.Name,
            Price = productViewModel.Price,

            ImageUrl = filePath,
            ImageId = fileStorage,

            LastPurchase = productViewModel.LastPurchase,
            LastSale = productViewModel.LastSale,
            IsAvailable = productViewModel.IsAvailable,
            Stock = productViewModel.Stock,
            User = productViewModel.User,
            WasDeleted = false
        };
    }


    public ProductViewModel ToProductViewModel(Product product)
    {
        return new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,

            ImageUrl = product.ImageUrl,
            ImageId = product.ImageId,

            LastPurchase = product.LastPurchase,
            LastSale = product.LastSale,
            IsAvailable = product.IsAvailable,
            Stock = product.Stock,
            User = product.User,
            WasDeleted = false
        };
    }
}