using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Models
{
    public class ProductViewModal : Product
    {
        [DisplayName("Image")] public IFormFile ImageFile { get; set; }
    }
}