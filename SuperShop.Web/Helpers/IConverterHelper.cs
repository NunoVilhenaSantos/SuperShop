using System;
using SuperShop.Web.Data.Entity;
using SuperShop.Web.Models;

namespace SuperShop.Web.Helpers
{
    public interface IConverterHelper
    {
        Product ToProduct(ProductViewModel productViewModel,
            string filePath, Guid fileStorage, bool isNew);


        ProductViewModel ToProductViewModel(Product product);
    }
}