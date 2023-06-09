using System;
using SuperShop.Web.Data.Entity;
using SuperShop.Web.Models;

namespace SuperShop.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Product ToProduct(ProductViewModel productViewModel,
            string filePath, Guid fileStorage, bool isNew)
        {
            return new Product
            {
                Id = isNew ? 0 : productViewModel.Id,
                Name = productViewModel.Name,
                Price = productViewModel.Price,

                ImageUrl = filePath,
                // Property or indexer 'property' cannot be assigned to -- it is read only
                // ImageFullUrl = string.Empty,
                ImageId = fileStorage,
                // Property or indexer 'property' cannot be assigned to -- it is read only
                // ImageFullIdUrl = string.Empty,
                ImageIdGcp = fileStorage,
                // Property or indexer 'property' cannot be assigned to -- it is read only
                // ImageFullIdGcpUrl = string.Empty,
                ImageIdAws = fileStorage,
                // Property or indexer 'property' cannot be assigned to -- it is read only
                // ImageFullIdAwsUrl = string.Empty,
                
                LastPurchase = productViewModel.LastPurchase,
                LastSale = productViewModel.LastSale,
                IsAvailable = productViewModel.IsAvailable,
                Stock = productViewModel.Stock,
                User = productViewModel.User

                                   
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
                // Property or indexer 'property' cannot be assigned to -- it is read only
                // ImageFullUrl = string.Empty,
                ImageId = product.ImageId,
                // Property or indexer 'property' cannot be assigned to -- it is read only
                // ImageFullIdUrl = string.Empty,
                ImageIdGcp = product.ImageId,
                // Property or indexer 'property' cannot be assigned to -- it is read only
                // ImageFullIdGcpUrl = string.Empty,
                ImageIdAws = product.ImageId,
                // Property or indexer 'property' cannot be assigned to -- it is read only
                // ImageFullIdAwsUrl = string.Empty,

                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                IsAvailable = product.IsAvailable,
                Stock = product.Stock,
                User = product.User
            };
        }
    }
}