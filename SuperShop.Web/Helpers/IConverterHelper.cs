using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Helpers
{
    public interface IConverterHelper
    {
        Product ToProduct(Models.ProductViewModel productViewModel,
            string filePath, bool isNew);


        Models.ProductViewModel ToProductViewModel(Product product);
    }
}