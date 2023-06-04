using SuperShop.Web.Data.Entity;
using SuperShop.Web.Models;

namespace SuperShop.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Product ToProduct(ProductViewModel productViewModel,
            string filePath, bool isNew)
        {
            return new Product
            {
                Id = isNew ? 0 : productViewModel.Id,
                ImageUrl = filePath,
                IsAvailable = productViewModel.IsAvailable,
                LastPurchase = productViewModel.LastPurchase,
                LastSale = productViewModel.LastSale,
                Name = productViewModel.Name,
                Price = productViewModel.Price,
                Stock = productViewModel.Stock,
                User = productViewModel.User
            };
        }

        public ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
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
    }
}